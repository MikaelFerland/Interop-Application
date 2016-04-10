using Interop.Infrastructure.Interfaces;

using System;
using System.ServiceModel;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Modules.Client.Services
{
    public class TargetService : ITargetService
    {
        ServiceHost _serviceHost;
        
        public TargetService()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            binding.Security.Mode = SecurityMode.None;
            

            Uri baseAddress = new Uri("net.tcp://192.168.1.95:8000/targetserver");
            _serviceHost = new ServiceHost(typeof(Server.TargetServer), baseAddress);
            
            _serviceHost.AddServiceEndpoint(typeof(ITargetServer), binding, baseAddress);

            _serviceHost.Open();
                
            Console.WriteLine(String.Format("The Target server is ready at {0}.", baseAddress));
        }
    }
}
