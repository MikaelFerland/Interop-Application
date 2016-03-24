using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;

using GMap.NET;
using GMap.NET.MapProviders;

using Interop.Infrastructure.Events;

namespace Interop.Modules.Obstacles.ViewModels
{
    public class ObstaclesViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        readonly Views.ObstaclesView _view;
        
        public ObstaclesViewModel(IEventAggregator eventAggregator, IRegion region)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            if (region == null)
            {
                throw new ArgumentNullException("region");
            }
            _eventAggregator = eventAggregator;
                        
            Provider = GMapProviders.OpenStreetMap;
            
            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Subscribe(Update_Obstacles);
        }

        public void Update_Obstacles(Infrastructure.Models.Obstacles obstacles)
        {
            this.Position = new PointLatLng(obstacles.moving_obstacles[0].latitude, obstacles.moving_obstacles[0].longitude);
        }

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
