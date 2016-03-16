using Prism.Modularity;
using Prism.Regions;
using System;

namespace Interop.Modules.Client
{
    public class ClientModule : IModule
    {
        IRegionManager _regionManager;

        public ClientModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //Will be a service
        }
    }
}