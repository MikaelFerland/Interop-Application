using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Interop.Infrastructure.Models
{
    public class Mission
    {
        [JsonProperty("id")]
        private int id { get; set; } //"id": 1,

        [JsonProperty("active")]
        bool active { get; set; } //"active": true,

        [JsonProperty("air_drop_pos")]
        air_drop_pos air_drop_pos { get; set; }

        [JsonProperty("fly_zones")]
        List<fly_zones> fly_zones { get; set; }

        [JsonProperty("home_pos")]
        home_pos home_pos { get; set; }

        [JsonProperty("mission_waypoints")]
        List<mission_waypoints> mission_waypoints { get; set; }

        [JsonProperty("off_axis_target_pos")]
        off_axis_target_pos off_axis_target_pos { get; set; }

        [JsonProperty("emergent_last_known_pos")]
        emergent_last_known_pos emergent_last_known_pos { get; set; }

        [JsonProperty("search_grid_points")]
        List<search_grid_points> search_grid_points { get; set; }
    }

    public class air_drop_pos
    {
        public double latitude { get; set; } //"latitude": 38.141833,
        public double longitude { get; set; } //"longitude": -76.425263
    }

    public class fly_zones
    {
        public double altitude_msl_max { get; set; } //"altitude_msl_max": 200.0,
        public double altitude_msl_min { get; set; } //"altitude_msl_min": 100.0,

        [JsonProperty("boundary_pts")]
        List<boundary_pts> boundary_pts { get; set; }
    }

    public class boundary_pts
    {
        public double latitude { get; set; } //"latitude": 38.142544,
        public double longitude { get; set; } //"longitude": -76.434088,

        [JsonProperty("order")]
        int order { get; set; } //"order": 1
    }

    public class home_pos
    {
        public double latitude { get; set; } //"latitude": 38.14792,
        public double longitude { get; set; } //"longitude": -76.427995
    }

    public class mission_waypoints
    {
        public double altitude_msl { get; set; } //"altitude_msl": 200.0,
        public double latitude { get; set; } //"latitude": 38.142544,
        public double longitude { get; set; } //"longitude": -76.434088,
        public int order { get; set; } //"order": 1
    }

    public class off_axis_target_pos
    {
        public double latitude { get; set; } //"latitude": 38.142544,
        public double longitude { get; set; } //"longitude": -76.434088
    }

    public class emergent_last_known_pos
    {
        public double latitude { get; set; }  //"latitude": 38.145823,
        public double longitude { get; set; } //"longitude": -76.422396 //"longitude": -76.434088
    }

    public class search_grid_points
    {
        public double altitude_msl { get; set; } //"altitude_msl": 200.0,
        public double latitude { get; set; } //"latitude": 38.142544,
        public double longitude { get; set; } //"longitude": -76.434088,
        public int order { get; set; } //"order": 1
    }
}
