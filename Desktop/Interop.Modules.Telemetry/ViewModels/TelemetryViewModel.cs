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
			DronePosition = $"{droneTelemetry.Latitutde.ToString()}, {droneTelemetry.Longitude.ToString()}";
            DroneHeading = $"{droneTelemetry.Heading.ToString()}";
        }

		/// <summary>
		/// 
		/// </summary>
		string _dronePosition = string.Empty;
		public string DronePosition
		{
			get
			{
				return _dronePosition;
			}
			set
			{
				if (SetProperty(ref _dronePosition, value))
				{
					//this.OnPropertyChanged(() => this.);
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        string _droneHeading = string.Empty;
        public string DroneHeading
        {
            get
            {
                return _droneHeading;
            }
            set
            {
                if (SetProperty(ref _droneHeading, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }
    }
}
