using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.Obstacles.Views;

namespace Interop.Modules.Obstacles
{
    public class ObstaclesModule : IModule
    {
        IRegionManager _regionManager;

        public ObstaclesModule(RegionManager regionManager)
            {
                _regionManager = regionManager;
            }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ObstaclesRegion", typeof(ObstaclesView));
        }
    }
}