using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

using GMap.NET;
using GMap.NET.MapProviders;

using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;

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

            Title = "Obstacles Region";
            Provider = GMapProviders.OpenStreetMap;
            
            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Subscribe(Update_Obstacles);
        }

        public void Update_Obstacles(Infrastructure.Models.Obstacles obstacles)
        {
            this.Position = new PointLatLng(obstacles.moving_obstacles[0].latitude, obstacles.moving_obstacles[0].longitude);
        }

        public string Title { get; set; }
        public GMapProvider Provider {get; set; }

        PointLatLng _position = new PointLatLng(45.4946761, -73.5622961);
        public PointLatLng Position
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
