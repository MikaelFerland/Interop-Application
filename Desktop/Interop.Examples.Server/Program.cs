using Interop.Infrastructure.Interfaces;

using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using Newtonsoft.Json;

namespace Interop.Examples.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost _serviceHost;

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            string myIP = GetLocalIPAddress();

            Uri baseAddress = new Uri($"net.tcp://{myIP}:80/targetserver");
            _serviceHost = new ServiceHost(typeof(TargetServer), baseAddress);
            _serviceHost.AddServiceEndpoint(typeof(ITargetServer), binding, baseAddress);
            
            _serviceHost.Open();
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Title = $"The Target server is ready at {baseAddress}.";
            Console.WriteLine("Waiting for message");
            while (true) { };
        }      

        // http://stackoverflow.com/questions/6803073/get-local-ip-address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }

    partial class TargetServer : ITargetServer
    {
        public Response SendTarget(InteropTargetMessage tInfo)
        {
            Response res = new Response();
            res.Message = "OK";

            CallbackTargetMessage(tInfo);
            return res;         
        }
        public void CallbackTargetMessage(InteropTargetMessage interopTargetMessage)
        {
            Console.Clear();
            string message = JsonConvert.SerializeObject(interopTargetMessage);
            Console.Write(message);
        }
    }
}
