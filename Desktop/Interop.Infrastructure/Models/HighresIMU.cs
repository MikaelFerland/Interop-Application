using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Models
{

    [Description("Packet id = 105")]
    public class HighresIMU
    {
        public int packet_id { get; set; }
        public ulong time_usec { get; set; }
        public float xacc { get; set; }
        public float yacc { get; set; }
        public float zacc { get; set; }
        public float xgyro { get; set; }
        public float ygyro { get; set; }
        public float zgyro { get; set; }
        public float xmag { get; set; }
        public float ymag { get; set; }
        public float zmag { get; set; }
        public float abs_pressure { get; set; }
        public float diff_pressure { get; set; }
        public float pressure_alt { get; set; }
        public float temperature { get; set; }
        public ushort temperafields_updated { get; set; }
    }
}
