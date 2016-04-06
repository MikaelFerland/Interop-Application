using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Modules.Telemetry.ViewModels
{
	public class TelemetryViewModel : BindableBase
	{
        IEventAggregator _eventAggregator;

        public TelemetryViewModel(IEventAggregator eventAggregator, ITelemetryService telemetryService)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            if (telemetryService == null)
            {
                throw new ArgumentNullException("telemetryService");
            }

            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<UpdateTelemetry>().Subscribe(Update_Telemetry);

            Title = "Telemetry Region";
        }

        public string Title { get; set; }

        public void Update_Telemetry(Infrastructure.Models.DroneTelemetry droneTelemetry)
        {

        }
    }
}
