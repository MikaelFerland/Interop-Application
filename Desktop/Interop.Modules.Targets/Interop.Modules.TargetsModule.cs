using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.Targets.Views;

namespace Interop.Modules.Targets
{
    public class TargetsModule : IModule
    {
        IRegionManager _regionManager;

        public TargetsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("TargetsRegion", typeof(TargetsView));
        }
    }
}