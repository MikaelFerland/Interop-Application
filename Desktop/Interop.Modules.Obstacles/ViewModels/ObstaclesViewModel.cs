using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;

using GMap.NET;
using GMap.NET.MapProviders;

using Interop.Infrastructure.Events;
using System.Windows;

namespace Interop.Modules.Obstacles.ViewModels
{
    public class ObstaclesViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        
        public ObstaclesViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;
            
            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Subscribe(Update_Obstacles);
            //TODO: Suscribe to Telemetry event to get the drono on the map
        }

        public void Update_Obstacles(Infrastructure.Models.Obstacles obstacles)
        {
            //TODO: Update each obstacles with their form on the map

            //Thid is for demo only it will be replace by the todo above
            this.Position = new Point(obstacles.moving_obstacles[0].latitude, obstacles.moving_obstacles[0].longitude);
        }

        /// <summary>
        /// This is the map provider that the control will use
        /// </summary>
        GMapProvider _provider = GMapProviders.OpenStreetMap;
        public GMapProvider Provider
        {
            get
            {
                return this._provider;
            }
            set
            {
                if (SetProperty(ref _provider, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        /// <summary>
        /// Represent the center of the map
        /// </summary>        
        Point _position = new Point(45.4946761, -73.5622961);
        public Point Position
        {
            get
            {
                return this._position;
            }
            set
            {
                if (SetProperty(ref _position, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }
    }
}
