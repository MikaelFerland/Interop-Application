using System.ComponentModel;

namespace Interop.Infrastructure.Models
{
    [Description("Packet id = 30")]
    public class Attitude
    {
        public uint packet_id { get; set; }
        public float time_boot_ms { get; set; }
        public float roll { get; set; }
        public float pitch { get; set; }
        public float yaw { get; set; }
        public float rollspeed { get; set; }
        public float pitchspeed { get; set; }
        public float yawspeed { get; set; }
    }
}
