using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.Login.Views;

namespace Interop.Modules.Login
{
    public class LoginModule : IModule
    {
        IRegionManager _regionManager;

        public LoginModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("SessionRegion", typeof(SessionStatusView));
        }
    }
}