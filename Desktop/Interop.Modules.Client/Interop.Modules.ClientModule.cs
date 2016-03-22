using Prism.Modularity;
using Prism.Regions;
using System;
using Prism.Unity;
using Microsoft.Practices.Unity;

using Interop.Infrastructure.Interfaces;

namespace Interop.Modules.Client
{
    public class ClientModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ClientModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            //Will be a service
            _container.RegisterType<IHttpService, Services.HttpService>();            
        }
    }
}