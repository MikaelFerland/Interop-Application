using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;

using Interop.Modules.Details.Views;

namespace Interop.Modules.Details
{
    public class DetailsModule : IModule
    {
        IRegionManager _regionManager;

        public DetailsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("DetailsRegion", typeof(TargetsView));
            _regionManager.RegisterViewWithRegion("DetailsRegion", typeof(ObstaclesView));
            _regionManager.RegisterViewWithRegion("DetailsRegion", typeof(MissionView));
        }
    }
}