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

namespace Interop.Infrastructure.Services
{
    public class HttpService : IHttpService
    {
        string USER = "testuser";
        string PASS = "testpass";
        string HOST = "http://192.168.99.100:8000";
        string REFRESH_RATE = "5";

        bool _isInitialized = false;
        static object[] REQUESTS =  {new GetTargets(), new GetObstacles(), new GetMissions() };
        ConcurrentDictionary<int, byte[]> _listOfImages = new ConcurrentDictionary<int, byte[]>();

        IEventAggregator _eventAggregator;
        CookieContainer _cookieContainer;

        Task<List<Target>> _targetsTask;
        Task<Obstacles> _obstaclesTask;
        Task<List<Mission>> _missionsTask;
        Task<bool> _isImagesLoadedTask;

        List<Task> tasks = new List<Task>();

        public HttpService(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            //var appSettings = ConfigurationManager.AppSettings;
            //USER = appSettings["USERNAME"];
            //PASS = appSettings["PASSWORD"];
            //HOST = appSettings["INTEROP_HOST"];
            //REFRESH_RATE = appSettings["REFRESH_RATE"];
                        
            //if (true == Login())
            //do
            //{
            //} while (true != Login());
            
            _eventAggregator.GetEvent<UpdateTelemetry>().Subscribe(TryPostDroneTelemetry, true);
            _eventAggregator.GetEvent<PostTargetEvent>().Subscribe(TryPostTarget, true);
            _eventAggregator.GetEvent<PutTargetEvent>().Subscribe(TryUpdateTarget, true);
            _eventAggregator.GetEvent<DeleteTargetEvent>().Subscribe(TryDeleteTarget, true);
        }

        ////TODO: Clean this method when the tests are done.
        //public async Task Run()
        //{
        //    //TODO: Latency monitoring, handle timeout if server is down. 

        //    try
        //    {
        //        if (!_isInitialized)
        //        {
        //            //RunAsync<List<Target>>((IRequest)REQUESTS[0]).ContinueWith(ctargetTask =>
        //            //{
        //            //    List<Target> listOfTarget = ctargetTask.Result;
        //            //    _eventAggregator.GetEvent<UpdateTargetsEvent>().Publish(listOfTarget);
        //            //    _isImagesLoadedTask = LoadImages(ctargetTask.Result);
        //            //    _targetsTask = RunAsync<List<Target>>((IRequest)REQUESTS[0]);
        //            //});


        //            await RunAsync<List<Target>>((IRequest)REQUESTS[0]).ContinueWith(ctargetTask =>
        //            {
        //                _isImagesLoadedTask = LoadImages(ctargetTask.Result);
        //            });

        //            await RunAsync<Obstacles>((IRequest)REQUESTS[1]).ContinueWith(ctargetTask =>
        //            {
        //                _eventAggregator.GetEvent<UpdateObstaclesEvent>().Publish(ctargetTask.Result);
        //            });

        //            await RunAsync<List<Mission>>((IRequest)REQUESTS[2]).ContinueWith(ctargetTask =>
        //            {
        //                _eventAggregator.GetEvent<UpdateMissionEvent>().Publish(ctargetTask.Result);
        //            });

        //            _isInitialized = true;
        //            return;
        //        }

        //        tasks.Add(Task.Run(() => RunAsync<List<Target>>((IRequest)REQUESTS[0]).ContinueWith(ctargetTask =>
        //        {
        //            _eventAggregator.GetEvent<UpdateTargetsEvent>().Publish(ctargetTask.Result);
        //            _isImagesLoadedTask = LoadImages(ctargetTask.Result);
        //        })));

        //        tasks.Add(Task.Run(() => RunAsync<Obstacles>((IRequest)REQUESTS[1]).ContinueWith(ctargetTask =>
        //        {
        //            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Publish(ctargetTask.Result);
        //        })));

        //        await Task.WhenAny(tasks);
        //        tasks.Clear();
        //    }
        //    catch (AggregateException aggEx)
        //    {
        //        var inners = aggEx.Flatten();
        //        Console.WriteLine("   {0}", inners.InnerException);
        //    }

        //    // You can decrease the value to get faster refresh
        //    if (REFRESH_RATE != string.Empty)
        //    {
        //        await Task.Delay(TimeSpan.FromMilliseconds(int.Parse(REFRESH_RATE))).ConfigureAwait(false);
        //    }

        //}

        public async Task Run()
        {
            //TODO: Latency monitoring, handle timeout if server is down. 

            try
            {

                if (!_isInitialized)
                {
                    _targetsTask = RunAsync<List<Target>>((IRequest)REQUESTS[0]);

                    _obstaclesTask = RunAsync<Obstacles>((IRequest)REQUESTS[1]);
                    _missionsTask = RunAsync<List<Mission>>((IRequest)REQUESTS[2]);
                    _isImagesLoadedTask = LoadImages(_targetsTask.Result);
                    _isInitialized = true;
                    //return Task.FromResult(true);
                }

                if (_targetsTask.IsCompleted)
                {
                    _eventAggregator.GetEvent<UpdateTargetsEvent>().Publish(_targetsTask.Result);
                    _isImagesLoadedTask = LoadImages(_targetsTask.Result);
                    _targetsTask = RunAsync<List<Target>>((IRequest)REQUESTS[0]);
                }

                if (_obstaclesTask.IsCompleted)
                {
                    _eventAggregator.GetEvent<UpdateObstaclesEvent>().Publish(_obstaclesTask.Result);
                    _obstaclesTask = RunAsync<Obstacles>((IRequest)REQUESTS[1]);
                }
                
                if (_missionsTask != null && _missionsTask.IsCompleted)
                {
                    _eventAggregator.GetEvent<UpdateMissionEvent>().Publish(_missionsTask.Result);
                    _missionsTask = null;
                }

            }
            catch (AggregateException aggEx)
            {
                var inners = aggEx.Flatten();
                Console.WriteLine("   {0}", inners.InnerException);
            }

            // You can decrease the value to get faster refresh
            if (REFRESH_RATE != string.Empty)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(int.Parse(REFRESH_RATE))).ConfigureAwait(false);
            }
            //return Task.FromResult(true);
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

        public bool Login(string username, string password)
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
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
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
