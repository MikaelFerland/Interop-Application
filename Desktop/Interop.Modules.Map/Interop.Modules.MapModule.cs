using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.Map.Views;

namespace Interop.Modules.Map
{
    public class MapModule : IModule
    {
        IRegionManager _regionManager;

        public MapModule(RegionManager regionManager)
            {
                _regionManager = regionManager;
            }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(MapView));
        }
    }
}