using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.Server.Views;

namespace Interop.Modules.Server
{
    public class ServerModule : IModule
    {
        IRegionManager _regionManager;

        public ServerModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ServerMessagesRegion", typeof(ServerInfoView));
        }
    }
}