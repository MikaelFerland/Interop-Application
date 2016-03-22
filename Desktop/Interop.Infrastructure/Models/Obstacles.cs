using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Models
{
    public class Obstacles
    {
        public List<moving_obstacles> moving_obstacles { get; set; }
        public List<stationary_obstacles> stationary_obstacles { get; set; }
    }

    //stationary_obstacles
    public class stationary_obstacles
    {
        public double cylinder_height { get; set; } //"cylinder_height": 750.0,
        public double cylinder_radius { get; set; } //"cylinder_radius": 300.0,
        public double latitude { get; set; }        //"latitude": 38.140578,
        public double longitude { get; set; }       //"longitude": -76.428997
    }

    //moving_obstacles
    public class moving_obstacles
    {
        public double altitude_msl { get; set; }  //"altitude_msl": 189.56748784643966,
        public double latitude { get; set; }      //"latitude": 38.141826869853645,
        public double longitude { get; set; }     //"longitude": -76.43199876559223,
        public double sphere_radius { get; set; } //"sphere_radius": 150.0
    }
}
