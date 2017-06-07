using System.ComponentModel;
using MavLinkNet;

namespace Interop.Infrastructure.Models
{
    public class DroneTelemetry
    {
        [Description("Packet id = 24")]
        public UasGpsRawInt GpsRawInt { get; set; }

        [Description("Packet id = 30")]
        public UasAttitude  Attitude { get; set; }

        [Description("Packet id = 33")]
        public UasGlobalPositionInt GlobalPositionInt { get; set; }

        [Description("Packet id = 105")]
        public UasHighresImu HighresIMU { get; set; }

        [Description("Packet id = 141")]
        public UasAltitude   Altitude { get; set; }

        [Description("Packet id = 74")]
        public UasVfrHud VfrHUD { get; set; }

        public float Latitutde
        {
            get
            {
                int lat = this.GlobalPositionInt.Lat;
                double corrLat = (double)lat * 1e-7;
                
                return (float)corrLat;
            }
        }

        public float Longitude
        {
            get
            {
                int lon = this.GlobalPositionInt.Lon;
                double corrLon = (double)lon * 1e-7;

                return (float)corrLon;
            }
        }

        public float AltitudeMSL
        {
            get
            {
                double alt_metric_mm = this.GlobalPositionInt.Alt;
                double alt_imperial_feet = alt_metric_mm * 0.00328084;

                return (float)alt_imperial_feet;
            }
        }

        public float Heading
        {
            get
            {
                return this.VfrHUD.Heading;
            }
        }
    }
}
