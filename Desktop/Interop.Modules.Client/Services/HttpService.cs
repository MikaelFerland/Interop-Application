using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;
using Interop.Modules.Client.Requests;

using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Interop.Modules.Client.Services
{
    public class HttpService : IHttpService
    {
        string USER = "simpleuser";
        string PASS = "simplepass";
        string HOST = "http://mikaelferland.com:80/";
        string REFRESH_RATE = "500";

        static object[] REQUESTS = {new GetTargets(), new GetObstacles() };
        ConcurrentDictionary<int, byte[]> _listOfImages = new ConcurrentDictionary<int, byte[]>();
        BackgroundWorker bw = new BackgroundWorker();

        IEventAggregator _eventAggregator;
        CookieContainer _cookieContainer;

        public HttpService(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            var appSettings = ConfigurationManager.AppSettings;
            USER = appSettings["USERNAME"];
            PASS = appSettings["PASSWORD"];
            HOST = appSettings["INTEROP_HOST"];
            REFRESH_RATE = appSettings["REFRESH_RATE"];
                        
            //if (true == Login())
            do
            {
            } while (true != Login());


            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            bw.DoWork += Bw_DoWork;
            bw.RunWorkerAsync();

            _eventAggregator.GetEvent<UpdateLoginStatusEvent>().Publish(string.Format("Connected as {0}", USER));

            _eventAggregator.GetEvent<UpdateTelemetry>().Subscribe(TryPostDroneTelemetry, true);
            _eventAggregator.GetEvent<PostTargetEvent>().Subscribe(TryPostTarget, true);
            _eventAggregator.GetEvent<PutTargetEvent>().Subscribe(TryUpdateTarget, true);
            _eventAggregator.GetEvent<DeleteTargetEvent>().Subscribe(TryDeleteTarget, true);
            
        }

        //TODO: Clean this method when the tests are done.
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO: Latency monitoring, handle timeout if server is down.            
            while (!(sender as BackgroundWorker).CancellationPending)
            {
                Task<List<Target>> targetsTask;
                Task<Obstacles> obstaclesTask;
                Task<bool> isImagesLoaded;

                try
                {

                    targetsTask = Task.Run(() =>
                    {
                        try
                        {
                            Task<List<Target>> temp = RunAsync<List<Target>>((IRequest)REQUESTS[1]);
                            return temp.Result;
                        }
                        catch (AggregateException ae)
                        {
                            //Console.WriteLine("One or more exceptions occurred: ");
                            //foreach (var ex in ae.Flatten().InnerExceptions)
                            //    Console.WriteLine("   {0}", ex.Message);
                            return null;
                        }
                    });

                    obstaclesTask = Task.Run(() =>
                    {
                        try
                        {
                            Task<Obstacles> temp = RunAsync<Obstacles>((IRequest)REQUESTS[2]);
                            return temp.Result;
                        }
                        catch (AggregateException ae)
                        {
                            //Console.WriteLine("One or more exceptions occurred: ");
                            //foreach (var ex in ae.Flatten().InnerExceptions)
                            //    Console.WriteLine("   {0}", ex.Message);
                            return null;
                        }
                    });

                    obstaclesTask = Task.Run(() =>
                    {
                        try
                        {
                            Task<Obstacles> temp = RunAsync<Obstacles>((IRequest)REQUESTS[2]);
                            return temp.Result;
                        }
                        catch (AggregateException ae)
                        {
                            //Console.WriteLine("One or more exceptions occurred: ");
                            //foreach (var ex in ae.Flatten().InnerExceptions)
                            //    Console.WriteLine("   {0}", ex.Message);
                            return null;
                        }

                    });

                    isImagesLoaded = Task.Run(() =>
                    {
                        try
                        {
                            Task<bool> temp = LoadImages(targetsTask.Result);
                            
                            return temp.Result;
                        }
                        catch (AggregateException ae)
                        {
                            //Console.WriteLine("One or more exceptions occurred: ");
                            //foreach (var ex in ae.Flatten().InnerExceptions)
                            //    Console.WriteLine("   {0}", ex.Message);
                            return false;
                        }
                    });

                    //serverInfoTask = Task.Run(() => RunAsync<ServerInfo>((IRequest)REQUESTS[0]).Result);
                    //targetsTask = Task.Run(() => RunAsync<List<Target>>((IRequest)REQUESTS[1]).Result);
                    //obstaclesTask = Task.Run(() => RunAsync<Obstacles>((IRequest)REQUESTS[2]).Result);
                    //
                    //Task<bool> isImagesLoaded = Task.Run(() => LoadImages(targetsTask.Result));

                    targetsTask.Wait();
                    obstaclesTask.Wait();

                    if (targetsTask?.Result != null)
                        _eventAggregator.GetEvent<UpdateTargetsEvent>().Publish(targetsTask.Result);
                    if (obstaclesTask?.Result != null)
                        _eventAggregator.GetEvent<UpdateObstaclesEvent>().Publish(obstaclesTask.Result);
                }
                catch (AggregateException ae)
                {
                    //Console.WriteLine("One or more exceptions occurred: ");
                    //foreach (var ex in ae.Flatten().InnerExceptions)
                    //    Console.WriteLine("   {0}", ex.Message);
                }

                //Console.WriteLine(serverInfoTask.Result.server_time);
                //Console.WriteLine(targetsTask.Result);
                //Console.WriteLine(obstaclesTask.Result);

                // You can decrease the value to get faster refresh
                if (REFRESH_RATE !=string.Empty)
                {
                    
                    Thread.Sleep(Int32.Parse(REFRESH_RATE));
                }
            }
        }

        private async Task<T> RunAsync<T>(IRequest request) where T : class
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                HttpResponseMessage response = await client.GetAsync(request.Endpoint);
                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();
                    request.Data = JsonConvert.DeserializeObject<T>(output);
                    return request.Data as T;
                }
            }
            return null;
        }

        public bool Login()
        {
            try
            {
                var baseAddress = new Uri(HOST);

                _cookieContainer = new CookieContainer();

                using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })

                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("username", USER),
                    new KeyValuePair<string, string>("password", PASS),
                });
                    _cookieContainer.Add(baseAddress, new Cookie("CookieName", "cookie_value"));
                    var result = client.PostAsync("/api/login", content).Result;
                    result.EnsureSuccessStatusCode();
                    return result.IsSuccessStatusCode;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private Task ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult(true);
        }

        public async void TryPostDroneTelemetry(DroneTelemetry droneTelemetry)
        {
            bool isPosted = false;
            ExceptionDispatchInfo capturedException = null;

            if (this._cookieContainer != null && droneTelemetry.GlobalPositionInt != null)
            {
                try
                {
                    isPosted = await PostDroneTelemetry(droneTelemetry);
                }
                catch (Exception ex)
                {
                    capturedException = ExceptionDispatchInfo.Capture(ex);
                }

                if (capturedException != null)
                {
                    await ExceptionHandler(capturedException.SourceException);
                }
            }
        }

        public async void TryPostTarget(InteropTargetMessage interopTargetMessage)
        {
            bool isTargetPosted = false;
            ExceptionDispatchInfo capturedException = null;

            try
            {
                if (interopTargetMessage != null)
                {
                    isTargetPosted = await PostTarget(interopTargetMessage);
                }
            }
            catch (Exception ex)
            {
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ExceptionHandler(capturedException.SourceException);
            }
        }

        public async void TryUpdateTarget(InteropTargetMessage interopTargetMessage)
        {            
            bool isTargetUpdated = false;
            ExceptionDispatchInfo capturedException = null;

            try
            {
                if (interopTargetMessage != null)
                {
                    isTargetUpdated = await PutTarget(interopTargetMessage);
                }
            }
            catch (Exception ex)
            {
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ExceptionHandler(capturedException.SourceException);
            }
        }

        public async void TryDeleteTarget(int interopId)
        {
            bool isTargetDeleted = false;
            ExceptionDispatchInfo capturedException = null;

            try
            {
                if (interopId > -1)
                {
                    isTargetDeleted = await DeleteTarget(interopId);
                }
            }
            catch (Exception ex)
            {
                capturedException = ExceptionDispatchInfo.Capture(ex);
            }

            if (capturedException != null)
            {
                await ExceptionHandler(capturedException.SourceException);
            }
        }

        public async Task<byte[]> LoadImage(int Id)
        {
            BitmapImage bitmapImage = new BitmapImage();
            byte[] emptyImage = new Byte[1];

            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })
            using (HttpClient client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));

                using (var response = await client.GetAsync($"/api/targets/{Id}/image"))
                {
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode == true)
                    {
                        byte[] inputStream = await response.Content.ReadAsByteArrayAsync();
                        return inputStream;
                    }
                }
            }
            return emptyImage;
        }

        public async Task<bool> LoadImages(List<Target> targets)
        {
            bool isImagesLoaded = false;
            foreach (Target target in targets)
            {
                if (!this._listOfImages.ContainsKey(target.id))
                {
                    byte[] imageBytes = await LoadImage(target.id);
                    _listOfImages.TryAdd(target.id, imageBytes);
                    isImagesLoaded = true;
                }
            }
            _eventAggregator.GetEvent<TargetImagesEvent>().Publish(_listOfImages);
            return isImagesLoaded;
        }

        public async Task<bool> PostDroneTelemetry(DroneTelemetry droneTelemetry)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("latitude", droneTelemetry.Latitutde.ToString()),
                    new KeyValuePair<string, string>("longitude", droneTelemetry.Longitude.ToString()),
                    new KeyValuePair<string, string>("altitude_msl", droneTelemetry.AltitudeMSL.ToString()),
                    new KeyValuePair<string, string>("uas_heading", droneTelemetry.Heading.ToString()),
                });

                var result = (await client.PostAsync("/api/telemetry", content));
                result.EnsureSuccessStatusCode();
                return result.IsSuccessStatusCode;
            }
        }

        public async Task<bool> PostTarget(InteropTargetMessage interopMessage)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);
                Target newTarget = new Target();

                newTarget.type = interopMessage.TargetType.ToString();
                newTarget.latitude = interopMessage.Latitude;
                newTarget.longitude = interopMessage.Longitude;
                newTarget.orientation = interopMessage.Orientation.ToString();
                newTarget.shape = interopMessage.Shape.ToString();
                newTarget.background_color = interopMessage.BackgroundColor.ToString();
                newTarget.alphanumeric_color = interopMessage.ForegroundColor.ToString();
                newTarget.alphanumeric = interopMessage.Character.ToString();
                newTarget.description = interopMessage.Description;

                var result = await client.PostAsync("/api/targets", new StringContent(JsonConvert.SerializeObject(newTarget)));
                Task<string> serverResponse = result.Content.ReadAsStringAsync();

                Target createdTarget = JsonConvert.DeserializeObject<Target>(serverResponse.Result);
                if (createdTarget != null)
                {
                    _eventAggregator.GetEvent<SetTargetIdEvent>().Publish(createdTarget.id);
                    await PostImage(interopMessage, createdTarget.id);
                }

                result.EnsureSuccessStatusCode();
                return result.IsSuccessStatusCode;
            }
        }

        public async Task<bool> PostImage(InteropTargetMessage interopMessage, int targetId)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);
                var imageBytes = new ByteArrayContent(interopMessage.Image);
                imageBytes.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                var result = await client.PostAsync($"/api/targets/{targetId}/image", imageBytes);
                result.EnsureSuccessStatusCode();
                return result.IsSuccessStatusCode;
            }
        }

        public async Task<bool> PutTarget(InteropTargetMessage interopMessage)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);

                Target newTarget = new Target();

                newTarget.type = interopMessage.TargetType.ToString();
                newTarget.latitude = interopMessage.Latitude;
                newTarget.longitude = interopMessage.Longitude;
                newTarget.orientation = interopMessage.Orientation.ToString();
                newTarget.shape = interopMessage.Shape.ToString();
                newTarget.background_color = interopMessage.BackgroundColor.ToString();
                newTarget.alphanumeric_color = interopMessage.ForegroundColor.ToString();
                newTarget.alphanumeric = interopMessage.Character.ToString();
                newTarget.description = interopMessage.Description.ToString();
                
                var result = await client.PutAsync($"/api/targets/{interopMessage.InteropID}", new StringContent(JsonConvert.SerializeObject(newTarget)));
                result.EnsureSuccessStatusCode();
                return result.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteTarget(int interopId)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = _cookieContainer })

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(HOST);

                var result = await client.DeleteAsync($"/api/targets/{interopId}");
                result.EnsureSuccessStatusCode();
                return result.IsSuccessStatusCode;
            }
        }
    }
}
