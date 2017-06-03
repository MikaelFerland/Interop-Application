using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.UserInterface.Views;

namespace Interop.Modules.UserInterface
{
    public class UserInterfaceModule : IModule
    {
        IRegionManager _regionManager;

        public UserInterfaceModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("SessionRegion", typeof(SessionStatusView));
            _regionManager.RegisterViewWithRegion("MenuRegion", typeof(MenuView));
        }
    }
}