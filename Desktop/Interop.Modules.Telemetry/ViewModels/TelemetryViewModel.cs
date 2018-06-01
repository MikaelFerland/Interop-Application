using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Timers;

namespace Interop.Modules.Telemetry.ViewModels
{
	public class TelemetryViewModel : BindableBase
	{
		IEventAggregator _eventAggregator;
		static Timer _watchdog;

		public TelemetryViewModel(IEventAggregator eventAggregator, ITelemetryService mavlinkService)
		{
			if (eventAggregator == null)
			{
				throw new ArgumentNullException("eventAggregator");
			}

			if (mavlinkService == null)
			{
				throw new ArgumentNullException("telemetryService");
			}

			_eventAggregator = eventAggregator;

			_watchdog = new Timer(1000);
			_watchdog.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            
			_eventAggregator.GetEvent<UpdateTelemetry>().Subscribe(Update_Telemetry, ThreadOption.UIThread, true);
			Title = "Telemetry Region";
		}

		public string Title { get; set; }

        public void Update_Telemetry(DroneTelemetry droneTelemetry)
        {
            bool isTelemetryUpdated = false;

            if (droneTelemetry.GlobalPositionInt != null)
            {
                DronePosition = $"{droneTelemetry.Latitude.ToString()}, {droneTelemetry.Longitude.ToString()}";
                DroneAltitude = $"{droneTelemetry.AltitudeMSL.ToString()}, feet";
                isTelemetryUpdated = true;
            }

            if (droneTelemetry.VfrHUD != null)
            {
                DroneHeading = $"{droneTelemetry.Heading.ToString()}";
                isTelemetryUpdated = true;
            }

            if (isTelemetryUpdated == true)
            {
                _watchdog.Enabled = false;
                _watchdog.Start();
                _watchdog.Enabled = true;
                this.IsDroneOnline = true;
            }
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.IsDroneOnline = false;
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

		/// <summary>
		/// 
		/// </summary>
		string _droneAltitude = string.Empty;
		public string DroneAltitude
		{
			get
			{
				return _droneAltitude;
			}
			set
			{
				if (SetProperty(ref _droneAltitude, value))
				{
					//this.OnPropertyChanged(() => this.);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		bool _isDroneOnline = false;
		public bool IsDroneOnline
		{
			get
			{
				return _isDroneOnline;
			}
			set
			{
				if (SetProperty(ref _isDroneOnline, value))
				{
					//this.OnPropertyChanged(() => this.);
				}
			}
		}
	}
}
