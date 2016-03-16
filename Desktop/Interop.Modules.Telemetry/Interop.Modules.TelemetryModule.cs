using Prism.Modularity;
using Prism.Regions;
using System;

using Interop.Modules.Telemetry.Views;

namespace Interop.Modules.Telemetry
{
    public class TelemetryModule : IModule
    {
        IRegionManager _regionManager;

        public TelemetryModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("TelemetryRegion", typeof(TelemetryView));
        }
    }
}