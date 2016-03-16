using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Modules.Telemetry.ViewModels
{
	public class TelemetryViewModel : BindableBase
	{
        public TelemetryViewModel()
        {
            Title = "Telemetry Region";
        }

        public string Title { get; set; }
    }
}
