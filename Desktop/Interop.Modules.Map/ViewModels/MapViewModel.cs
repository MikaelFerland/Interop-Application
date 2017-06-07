using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Interop.Modules.Map.ViewModels
{
    public class MapViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        IHttpService _httpService;
        Views.Map _map;
        Views.MapView _view;
        List<PointLatLng> _polygonPointsLatLng = new List<PointLatLng>();
        List<PointLatLng> _area = new List<PointLatLng>();
        List<Target> _targets;
        Mission _currentMission;

        public MapViewModel(IEventAggregator eventAggregator, IHttpService httpService, IView view)
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
            _httpService = httpService;
            _view = (view as Views.MapView);
            _map = _view.Map;
                        
            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Subscribe(Update_Obstacles, ThreadOption.UIThread);
            _eventAggregator.GetEvent<UpdateTelemetry>().Subscribe(Update_DronePosition);
            _eventAggregator.GetEvent<UpdateTargetsEvent>().Subscribe(Update_TargetsLocation);
            _eventAggregator.GetEvent<UpdateSelectedMission>().Subscribe(Update_SelectedMission);
            //SetGeofence();
            //SetArea();
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
            SetObstacles(obstacles);
            
            //TODO: Remove the following line when the debug will be finish
            //this.Position = new Point(obstacles.moving_obstacles[0].latitude, obstacles.moving_obstacles[0].longitude);
        }
        public void Update_DronePosition(Infrastructure.Models.DroneTelemetry droneTelemetry)
        {
            if (droneTelemetry.GlobalPositionInt != null)
            {
                this.Position = new Point(droneTelemetry.Latitutde, droneTelemetry.Longitude);
            }
        }

        public void Update_TargetsLocation(List<Target> targets)
        {
            if (targets != null)
            {
                _targets = targets;
            }
        }
        public void Update_SelectedMission(Mission mission)
        {
            _currentMission = mission;
        }

        public PointLatLng ExtactLatLon(string combinedPoint)
        {
            if (combinedPoint != string.Empty)
            {
                var latLon = combinedPoint.Split(':');
                var pointLatLon = new PointLatLng(double.Parse(latLon[0]), double.Parse(latLon[1]));
                return pointLatLon;
            }
            return PointLatLng.Empty;
        }

        public void FetchGeoFencePoints(string dataFile, List<PointLatLng> listOfPoint)
        {
            _polygonPointsLatLng.Clear();

            using (StreamReader sr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + $"\\{dataFile}.data"))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    listOfPoint.Add(ExtactLatLon(s));
                }
            }
        }

        public void SetGeofenceFromFile()
        {
            
            FetchGeoFencePoints("geoFence", _polygonPointsLatLng);

            var geofence = new GMapPolygon(_polygonPointsLatLng);

            var polygon = new System.Windows.Shapes.Path();
            var strokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            strokeBrush.Opacity = 1;
            var fillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
            fillBrush.Opacity = 0.1;

            polygon.Fill = fillBrush;
            polygon.Stroke = strokeBrush;
            polygon.StrokeThickness = 3;

            geofence.Shape = polygon;
            geofence.RegenerateShape(_map);

            _map.Markers.Add(geofence);
        }
        public void SetGeofenceFromCurrentMission(Mission mission)
        {            
            if (mission == null) return;

            _polygonPointsLatLng.Clear();
            foreach (var flyZone in mission.FlyZones)
            {
                foreach (var boundaryPoint in flyZone.BoundaryPoints)
                {
                    _polygonPointsLatLng.Add(new PointLatLng { Lat=boundaryPoint.Latitude, Lng=boundaryPoint.Longitude });
                }
            }

            var geofence = new GMapPolygon(_polygonPointsLatLng);

            var polygon = new System.Windows.Shapes.Path();
            var strokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            strokeBrush.Opacity = 1;
            var fillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
            fillBrush.Opacity = 0.1;

            polygon.Fill = fillBrush;
            polygon.Stroke = strokeBrush;
            polygon.StrokeThickness = 3;

            geofence.Shape = polygon;
            geofence.RegenerateShape(_map);

            _map.Markers.Add(geofence);
        }

        public void SetAreaFromFile()
        {
            if (_area?.Count == 0)
            {
                FetchGeoFencePoints("area", _area);
            }
            
            var area = new GMapPolygon(_area);

            var polygon = new System.Windows.Shapes.Path();
            var strokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkBlue);
            strokeBrush.Opacity = 0.4;
            var fillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkBlue);
            fillBrush.Opacity = 0.4;

            polygon.Fill = fillBrush;
            polygon.Stroke = strokeBrush;
            polygon.StrokeThickness = 3;

            area.Shape = polygon;
            area.RegenerateShape(_map);

            _map.Markers.Add(area);
        }
        public void SetAreaFromCurrentMission(Mission mission)
        {
            if (mission == null) return;

            _area.Clear();
            foreach (var searchGridPoint in mission.SearchGridPoints)
            {
                var gpsPoint = new PointLatLng { Lat = searchGridPoint.Latitude, Lng = searchGridPoint.Longitude };
                _area.Add(gpsPoint);
                AddMarker(searchGridPoint , System.Windows.Media.Brushes.Green);
            }

            var area = new GMapPolygon(_area);
            
            var polygon = new System.Windows.Shapes.Path();
            var strokeBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkBlue);
            strokeBrush.Opacity = 0.4;
            var fillBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkBlue);
            fillBrush.Opacity = 0.4;

            polygon.Fill = fillBrush;
            polygon.Stroke = strokeBrush;
            polygon.StrokeThickness = 3;

            area.Shape = polygon;
            area.RegenerateShape(_map);

            _map.Markers.Add(area);
        }
        public void SetTargets()
        {
            if (_targets != null && _targets.Count > 0)
            {
                foreach (Target target in _targets)
                {
                    var marker = new GMapMarker(new PointLatLng(target.latitude, target.longitude));
                    var res = scaleDimension(target.latitude, _map.Zoom, 0.3 * 12.0);

                    var grid = new Grid();
                    var shape = new System.Windows.Shapes.Rectangle();
                    shape.Height = res * 2;
                    shape.Width = res * 2;
                    shape.Fill = System.Windows.Media.Brushes.White;
                    shape.Opacity = 10;

                    var label = new Label();
                    label.Content = target.id;
                    label.FontWeight = FontWeights.Bold;
                    label.HorizontalAlignment = HorizontalAlignment.Center;
                    label.VerticalAlignment = VerticalAlignment.Center;

                    grid.Children.Add(shape);
                    grid.Children.Add(label);

                    marker.Offset = new Point(-res, -res);
                    marker.Shape = grid;
                    marker.ZIndex = 1;                    
                    _map.Markers.Add(marker);
                }                
            }
        }

        public void SetMission(Mission mission)
        {
            if (mission == null) return;

            AddMarker(mission.HomePosition, System.Windows.Media.Brushes.White);
            AddMarker(mission.AirDropPosition, System.Windows.Media.Brushes.White);
            AddMarker(mission.OffAxisTargetPosition, System.Windows.Media.Brushes.White);
            AddMarker(mission.EmergentLastKnownPosition, System.Windows.Media.Brushes.White);

            foreach (var waypoint in mission.Waypoints)
            {
                AddMarker(waypoint, System.Windows.Media.Brushes.White);
            }
        }

        void AddMarker(BasePoint gpsPoint, System.Windows.Media.Brush color)
        {
            var p = new PointLatLng { Lat = gpsPoint.Latitude, Lng = gpsPoint.Longitude };
            var marker = new GMapMarker(p);
            var res = scaleDimension(p.Lat, _map.Zoom, 0.3 * 12.0);
            
            var grid = new Grid();

            var label = new Label();
            label.Content = gpsPoint.Tag;
            label.FontWeight = FontWeights.Bold;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;

            var shape = new System.Windows.Shapes.Rectangle();
            shape.Height = label.Height/2;
            shape.Width = label.Width;

            if (gpsPoint.IsSelected)
            {
                shape.Fill = System.Windows.Media.Brushes.Red;
                shape.Opacity = 70;
                marker.ZIndex = 2;
            }
            else
            {
                shape.Fill = null;
                shape.Stroke = color;
                marker.ZIndex = 1;
            }
               
            grid.Children.Add(shape);
            grid.Children.Add(label);

            marker.Offset = new Point(-res, -res);
            marker.Shape = grid;
            marker.ZIndex = 1;
            _map.Markers.Add(marker);
        }

        public void SetObstacles(Infrastructure.Models.Obstacles obstacles)
        {
            if (obstacles != null)
            {
                try
                {

                    //if (Application.Current == null) return;
                    // We are able to modify the markers collection only if we are on the same thread
                    // http://stackoverflow.com/questions/18331723/this-type-of-collectionview-does-not-support-changes-to-its-sourcecollection-fro
                    //Application.Current.Dispatcher.Invoke((Action)delegate // <--- HERE                    
                    {
                        _map.Markers.Clear();

                        //Update static obstacles
                        foreach (var obstacle in obstacles.stationary_obstacles)
                        {
                            var marker = new GMapMarker(new PointLatLng(obstacle.latitude, obstacle.longitude));
                            var res = scaleDimension(obstacle.latitude, _map.Zoom, obstacle.cylinder_radius * 12.0);

                            var grid = new Grid();
                            var shape = new System.Windows.Shapes.Ellipse();                            
                            shape.Height = res * 2;
                            shape.Width = res * 2;
                            shape.Fill = System.Windows.Media.Brushes.Cyan;
                            shape.Opacity = 10;

                            var label = new Label();
                            label.Content = obstacle.cylinder_height;
                            label.FontWeight = FontWeights.Bold;
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;

                            grid.Children.Add(shape);
                            grid.Children.Add(label);

                            marker.Offset = new Point(-res, -res);
                            marker.Shape = grid;
                            marker.ZIndex = 0;
                            _map.Markers.Add(marker);
                        }

                        //Update the moving obstacles
                        foreach (var obstacle in obstacles.moving_obstacles)
                        {
                            var marker = new GMapMarker(new PointLatLng(obstacle.latitude, obstacle.longitude));
                            var res = scaleDimension(obstacle.latitude, _map.Zoom, obstacle.sphere_radius * 12.0);

                            var grid = new Grid();
                            var shape = new System.Windows.Shapes.Ellipse();
                            shape.Height = res * 2.0;
                            shape.Width = res * 2.0;
                            shape.Fill = System.Windows.Media.Brushes.Red;
                            shape.Opacity = 10;

                            var label = new Label();
                            label.Content = obstacle.altitude_msl.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
                            label.FontWeight = FontWeights.Bold;
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.VerticalAlignment = VerticalAlignment.Center;

                            grid.Children.Add(shape);
                            grid.Children.Add(label);

                            marker.Offset = new Point(-res, -res);
                            marker.Shape = grid;
                            marker.ZIndex = 0;
                            _map.Markers.Add(marker);
                        }

                        //SetGeofence();
                        SetGeofenceFromCurrentMission(_currentMission);
                        SetAreaFromCurrentMission(_currentMission);
                        SetTargets();
                        SetMission(_currentMission);
                        //SetArea();
                    }//);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
