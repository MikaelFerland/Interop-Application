using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Models
{

    [Description("Packet id = 141")]
    public class Altitude
    {
        public int packet_id { get; set; }
        public ulong time_usec { get; set; }
        public float altitude_monotonic { get; set; }
        public float altitude_amsl { get; set; }
        public float altitude_local { get; set; }
        public float altitude_relative { get; set; }
        public float altitude_terrain { get; set; }
        public float bottom_clearance { get; set; }
    }
}
