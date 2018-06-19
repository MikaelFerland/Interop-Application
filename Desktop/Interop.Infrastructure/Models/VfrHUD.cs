using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Models
{

    [Description("Packet id = 74")]
    public class VfrHUD
    {
        public float airspeed { get; set; }
        public float groundspeed { get; set; }
        public short heading { get; set; }
        public ushort throttle { get; set; }
        public float alt { get; set; }
        public float climb { get; set; }
    }
}
