using System.Collections.Generic;

using Newtonsoft.Json;

namespace Interop.Infrastructure.Models
{
    public class Mission
    {
        public Mission()
        {
            
        }

        [JsonProperty("id")]
        public int Id { get; set; } //"id": 1,

        [JsonProperty("active")]
        public bool Active { get; set; } //"active": true,

        [JsonProperty("air_drop_pos")]
        public AirDropPosition AirDropPosition { get; set; }

        [JsonProperty("fly_zones")]
        public List<FlyZone> FlyZones { get; set; }

        [JsonProperty("home_pos")]
        public HomePosition HomePosition { get; set; }

        [JsonProperty("mission_waypoints")]
        public List<Waypoint> Waypoints { get; set; }

        [JsonProperty("off_axis_target_pos")]
        public OffAxisTargetPosition OffAxisTargetPosition { get; set; }

        [JsonProperty("emergent_last_known_pos")]
        public EmergentLastKnownPosition EmergentLastKnownPosition { get; set; }

        [JsonProperty("search_grid_points")]
        public List<SearchGridPoint> SearchGridPoints { get; set; }

        public List<BasePoint> GetAllGpsPoints()
        {
            var gpsPoints = new List<BasePoint>();

            if (this.HomePosition != null) gpsPoints.Add(this.HomePosition);

            if (this.Waypoints?.Count > 0)
            {
                foreach (var waypoint in this.Waypoints)
                {
                    gpsPoints.Add(waypoint);
                }
            }

            if(this.AirDropPosition != null) gpsPoints.Add(this.AirDropPosition);
            if(this.EmergentLastKnownPosition != null) gpsPoints.Add(this.EmergentLastKnownPosition);
            if(this.OffAxisTargetPosition != null) gpsPoints.Add(this.OffAxisTargetPosition);

            if (this.FlyZones?.Count > 0)
            {
                foreach (var flyZone in this.FlyZones)
                {
                    foreach(var boundaryPoint in flyZone.BoundaryPoints)
                    {
                        gpsPoints.Add(boundaryPoint);
                    }
                }
            }

            if (this.SearchGridPoints?.Count > 0)
            {
                foreach (var searchGridPoint in this.SearchGridPoints)
                {
                    gpsPoints.Add(searchGridPoint);
                }
            }

            return gpsPoints;
        }


    }

    public class AirDropPosition : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Single; }
        }
        public override string Tag
        {
            get { return "A"; }
        }
        public override string Description
        {
            get { return "Air Drop"; }
        }
        public override double Latitude { get; set; } //"latitude": 38.141833,
        public override double Longitude { get; set; } //"longitude": -76.425263
    }

    public class FlyZone
    {
        public PointType GpsType
        {
            get { return PointType.Area; }
        }
        public double Altitude_msl_max { get; set; } //"altitude_msl_max": 200.0,
        public double Altitude_msl_min { get; set; } //"altitude_msl_min": 100.0,

        [JsonProperty("boundary_pts")]
        public List<BoundaryPoint> BoundaryPoints { get; set; }
    }

    public class BoundaryPoint : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Area; }
        }
        public override string Tag
        {
            get { return "B" + Order; }
        }
        public override string Description
        {
            get { return "Boundary"; }
        }
        public override double Latitude { get; set; } //"latitude": 38.142544,
        public override double Longitude { get; set; } //"longitude": -76.434088,

        [JsonProperty("order")]
        public int Order { get; set; } //"order": 1
    }

    public class HomePosition : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Single; }
        }
        public override string Tag
        {
            get { return "H"; }
        }
        public override string Description
        {
            get { return "Home"; }
        }
        public override double Latitude { get; set; } //"latitude": 38.14792,
        public override double Longitude { get; set; } //"longitude": -76.427995
    }

    public class Waypoint : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Single; }
        }
        public override string Tag
        {
            get { return "W" + Order; }
        }
        public override string Description
        {
            get { return "Waypoint"; }
        }
        public double Altitude_msl { get; set; } //"altitude_msl": 200.0,
        public override double Latitude { get; set; } //"latitude": 38.142544,
        public override double Longitude { get; set; } //"longitude": -76.434088,
        public int Order { get; set; } //"order": 1
    }

    public class OffAxisTargetPosition : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Single; }
        }
        public override string Tag
        {
            get { return "O"; }
        }
        public override string Description
        {
            get { return "Off Axis Target"; }
        }
        public override double Latitude { get; set; } //"latitude": 38.142544,
        public override double Longitude { get; set; } //"longitude": -76.434088
    }

    public class EmergentLastKnownPosition : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Single; }
        }
        public override string Tag
        {
            get { return "E"; }
        }
        public override string Description
        {
            get { return "Emergent Last Know"; }
        }
        public override double Latitude { get; set; }  //"latitude": 38.145823,
        public override double Longitude { get; set; } //"longitude": -76.422396 //"longitude": -76.434088
    }

    public class SearchGridPoint : BasePoint
    {
        public override PointType GpsType
        {
            get { return PointType.Area; }
        }
        public override string Tag
        {
            get { return "S" + Order; }
        }
        public override string Description
        {
            get { return "Search Grid"; }
        }
        public double Altitude_msl { get; set; } //"altitude_msl": 200.0,
        public override double Latitude { get; set; } //"latitude": 38.142544,
        public override double Longitude { get; set; } //"longitude": -76.434088,
        public int Order { get; set; } //"order": 1
    }

    public enum PointType
    {
        Single,
        Area
    }

    public abstract class BasePoint
    {
        virtual public PointType GpsType { get; }
        virtual public string Tag { get; }
        virtual public string Description { get; }
        virtual public double Latitude { get; set; }
        virtual public double Longitude { get; set; }
    }
}
