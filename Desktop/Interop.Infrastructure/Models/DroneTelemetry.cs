using System.ComponentModel;

namespace Interop.Infrastructure.Models
{
    public class DroneTelemetry
    {
        public DroneTelemetry()
        {
        }

        public GpsRawInt GpsRawInt { get; set; }
        public Attitude Attitude { get; set; }
        public GlobalPositionInt GlobalPositionInt { get; set; }
        public HighresIMU HighresIMU { get; set; }
        public Altitude Altitude { get; set; }
        public VfrHUD VfrHUD { get; set; }

        public float Latitude
        {
            get
            {
                var lat = this.GlobalPositionInt.lat;
                var corrLat = lat * 1e-7;

                return (float)corrLat;
            }
        }

        public float Longitude
        {
            get
            {
                var lon = this.GlobalPositionInt.lon;
                var corrLon = lon * 1e-7;

                return (float)corrLon;
            }
        }

        public float AltitudeMSL
        {
            get
            {
                double alt_metric_mm = this.GlobalPositionInt.alt;
                var alt_imperial_feet = alt_metric_mm * 0.00328084;

                return (float)alt_imperial_feet;
            }
        }

        public float Heading => GlobalPositionInt?.hdg / 1000f ?? 0;
    }
}
