using MavLinkNet;
using System.ComponentModel;
using System.Collections.Generic;

namespace Interop.Infrastructure.Models
{
    public class DroneTelemetry
    {
        public DroneTelemetry()
        {
            Messages = new System.Collections.Concurrent.ConcurrentDictionary<uint, UasMessage>();
        }

        public System.Collections.Concurrent.ConcurrentDictionary<uint, UasMessage> Messages { get; set; }

        public UasGpsRawInt GpsRawInt { get; set; }
        public UasAttitude Attitude { get; set; }
        public UasGlobalPositionInt GlobalPositionInt { get; set; }
        public UasHighresImu HighresIMU { get; set; }
        public UasAltitude Altitude { get; set; }
        public UasVfrHud VfrHUD { get; set; }

        public float Latitude
        {
            get
            {
                var lat = this.GlobalPositionInt.Lat;
                var corrLat = lat * 1e-7;

                return (float)corrLat;
            }
        }

        public float Longitude
        {
            get
            {
                var lon = this.GlobalPositionInt.Lon;
                var corrLon = lon * 1e-7;

                return (float)corrLon;
            }
        }

        public float AltitudeMSL
        {
            get
            {
                double alt_metric_mm = this.GlobalPositionInt.Alt;
                var alt_imperial_feet = alt_metric_mm * 0.00328084;

                return (float)alt_imperial_feet;
            }
        }

        public float Heading => GlobalPositionInt?.Hdg / 1000f ?? 0;
    }
}
