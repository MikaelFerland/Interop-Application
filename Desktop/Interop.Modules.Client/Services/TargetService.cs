using Interop.Infrastructure.Interfaces;

using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;

using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Interop.Modules.Client.Services
{
    public class TargetService : ITargetService
    {
        IEventAggregator _eventAggregator;

        ServiceHost _serviceHost;
        
        public TargetService(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            _eventAggregator = eventAggregator;


            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            string myIP = GetLocalIPAddress();

            Uri baseAddress = new Uri($"net.tcp://{myIP}:8000/targetserver");
            _serviceHost = new ServiceHost(typeof(Server.TargetServer), baseAddress);
            _serviceHost.AddServiceEndpoint(typeof(ITargetServer), binding, baseAddress);

            _serviceHost.Open();
                
            Console.WriteLine(String.Format("The Target server is ready at {0}.", baseAddress));
        }

        public static void CallbackTargetMessage(InteropTargetMessage interopTargetMessage)
        {
            Console.WriteLine(interopTargetMessage.Operation);
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
}
