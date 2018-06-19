using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Models
{

    [Description("Packet id = 33")]
    public class GlobalPositionInt
    {
        public int packet_id { get; set; }
        public uint time_boot_ms { get; set; }
        public int lat { get; set; }
        public int lon { get; set; }
        public int alt { get; set; }
        public int relative_alt { get; set; }
        public short vx { get; set; }
        public short vy { get; set; }
        public short vz { get; set; }
        public ushort hdg { get; set; }
    }
}
