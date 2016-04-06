using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Interop.Infrastructure.Models
{
    public class DroneTelemetry
    {
        public GpsRawInt GpsRawInt { get; set; }
        public Attitude  Attitude { get; set; }
        public GlobalPositionInt GlobalPositionInt { get; set; }
        public HighresIMU HighresIMU { get; set; }
        public Altitude   Altitude { get; set; }
    }

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
