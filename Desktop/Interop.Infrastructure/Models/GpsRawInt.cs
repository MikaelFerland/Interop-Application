using System.ComponentModel;

namespace Interop.Infrastructure.Models
{

    [Description("Packet id = 24")]
    public class GpsRawInt
    {
        public int packet_id { get; set; }
        public ulong time_usec { get; set; }
        public byte fix_type { get; set; }
        public int lat { get; set; }
        public int lon { get; set; }
        public int alt { get; set; }
        public ushort eph { get; set; }
        public ushort epv { get; set; }
        public ushort vel { get; set; }
        public ushort cog { get; set; }
        public byte satellites_visible { get; set; }
    }
}
