using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;

using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;

namespace Interop.Modules.Obstacles.ViewModels
{
    public class ObstaclesViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        Views.Map _map;
        Views.ObstaclesView _view;

        public ObstaclesViewModel(IEventAggregator eventAggregator, IView view)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            if (view == null)
            {
                throw new ArgumentNullException("obstaclesView");
            }

            _eventAggregator = eventAggregator;
            _view = (view as Views.ObstaclesView);
            _map = _view.Map;
                        
            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Subscribe(Update_Obstacles);
            _eventAggregator.GetEvent<UpdateTelemetry>().Subscribe(Update_DronePosition);
        }

        public double scaleDimension(double latitude, double zoomLevel, double mapDimension)
        {
            var DPI   = 1.0;
            var res   = 156543.03 * Math.Cos(latitude) / Math.Pow(2.0, zoomLevel);
            var scale = 1 / (DPI * 39.37 * res);

            return scale * mapDimension;
        }

        public void Update_Obstacles(Infrastructure.Models.Obstacles obstacles)
        {
            //TODO: Update each obstacles with their radius on the map
            SetObstacles(obstacles);

            //TODO: Remove the following line when the debug will be finish
            //this.Position = new Point(obstacles.moving_obstacles[0].latitude, obstacles.moving_obstacles[0].longitude);
        }

        public void SetObstacles(Infrastructure.Models.Obstacles obstacles)
        {
            if (obstacles != null)
            {
                try
                {
                    // We are able to modify the markers collection only if we are on the same thread
                    // http://stackoverflow.com/questions/18331723/this-type-of-collectionview-does-not-support-changes-to-its-sourcecollection-fro
                    Application.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        _map.Markers.Clear();

                        //Update static obstacles
                        foreach (var obstacle in obstacles.stationary_obstacles)
                        {
                            var marker = new GMapMarker(new PointLatLng(obstacle.latitude, obstacle.longitude));
                            var res = scaleDimension(obstacle.latitude, _map.Zoom, obstacle.cylinder_radius * 12.0);

                            var shape = new System.Windows.Shapes.Ellipse();
                            shape.Height = res * 2;
                            shape.Width = res * 2;
                            shape.Fill = System.Windows.Media.Brushes.Cyan;
                            shape.Opacity = 10;

                            marker.Offset = new Point(-res, -res);
                            marker.Shape = shape;
                            _map.Markers.Add(marker);
                        }

                        //Update the moving obstacles
                        foreach (var obstacle in obstacles.moving_obstacles)
                        {
                            var marker = new GMapMarker(new PointLatLng(obstacle.latitude, obstacle.longitude));
                            var res = scaleDimension(obstacle.latitude, _map.Zoom, obstacle.sphere_radius * 12.0);

                            var shape = new System.Windows.Shapes.Ellipse();
                            shape.Height = res * 2.0;
                            shape.Width = res * 2.0;
                            shape.Fill = System.Windows.Media.Brushes.Red;
                            shape.Opacity = 10;

                            marker.Offset = new Point(-res, -res);
                            marker.Shape = shape;
                            _map.Markers.Add(marker);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void Update_DronePosition(Infrastructure.Models.DroneTelemetry droneTelemetry)
        {   
            if (droneTelemetry.GlobalPositionInt != null)
            { 
                this.Position = new Point(droneTelemetry.Latitutde, droneTelemetry.Longitude);
            }
        }

        /// <summary>
        /// This is the map provider that the control will use
        /// </summary>
        GMapProvider _provider = GMapProviders.OviHybridMap;
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
