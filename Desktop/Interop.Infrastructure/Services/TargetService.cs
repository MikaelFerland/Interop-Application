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
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace Interop.Infrastructure.Services
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
            binding.MaxReceivedMessageSize = 30000000;

            string myIP = GetLocalIPAddress();
            
            Uri baseAddress = new Uri($"net.tcp://192.168.1.107:8000/targetserver");
            _serviceHost = new ServiceHost(typeof(Server.TargetServer), baseAddress);
            
            var instanceProvider = new InstanceProviderBehavior<ITargetServer>(() => new Server.TargetServer(eventAggregator));
            _serviceHost.AddServiceEndpoint(typeof(ITargetServer), binding, baseAddress);
            instanceProvider.AddToAllContracts(_serviceHost);

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
    public class InstanceProviderBehavior<T> : IInstanceProvider, IContractBehavior
        where T : class
    {
        private readonly Func<T> _instanceProvider;

        public InstanceProviderBehavior(Func<T> instanceProvider)
        {
            _instanceProvider = instanceProvider;
        }

        // I think this method is more suitable to be an extension method of ServiceHost.
        // I put it here in order to simplify the code.
        public void AddToAllContracts(ServiceHost serviceHost)
        {
            foreach (var endpoint in serviceHost.Description.Endpoints)
            {
                endpoint.Contract.Behaviors.Add(this);
            }
        }

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            // Create a new instance of T
            return _instanceProvider.Invoke();
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            try
            {
                var disposable = instance as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            catch { }
        }

        #endregion

        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
