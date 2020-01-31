using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;
using Prism.Events;
using MavLinkNet;

using System;

namespace Interop.Infrastructure.Services
{

    public class MavlinkService : IMavlinkService
    {
        private IEventAggregator _eventAggregator;
        private MavLinkUdpTransport _mavlink;
        private DroneTelemetry _droneTelemetry;

        public MavlinkService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            _droneTelemetry = new DroneTelemetry();
            _mavlink = new MavLinkUdpTransport();
            _mavlink.TargetIpAddress = new System.Net.IPAddress(new byte[] { 0, 0, 0, 0 });
            _mavlink.UdpListeningPort = 14552; // px4 14550 mavproxy 14552;
            _mavlink.OnPacketReceived += Mavlink_OnPacketReceived;
            _mavlink.Initialize();
        }

        ////TODO : Modify mavlink.net to be compatible with mavlink protocol v2
        private void Mavlink_OnPacketReceived(object sender, MavLinkPacketBase packet)
        {
            try
            {
                switch (packet.MessageId)
                {
                    case 24: // GPS_RAW_INT                        
                        _droneTelemetry.GpsRawInt = ExtractMessage<UasGpsRawInt>(packet);                        
                        break;
                    case 30: // ATTITUDE
                        _droneTelemetry.Attitude = ExtractMessage<UasAttitude>(packet);
                        break;
                    case 33: // GLOBAL_POSITION_INT
                        _droneTelemetry.GlobalPositionInt = ExtractMessage<UasGlobalPositionInt>(packet);
                        break;
                    case 74: // VFR_HUD
                        _droneTelemetry.VfrHUD = ExtractMessage<UasVfrHud>(packet);
                        break;
                    case 105: // HIGHRES_IMU
                        _droneTelemetry.HighresIMU = (UasHighresImu)packet.Message;
                        break;
                    case 141: // ALTITUDE
                        _droneTelemetry.Altitude = (UasAltitude)packet.Message;
                        break;

                    default: // NOT YET SUPPORTED
                        _droneTelemetry.Messages.AddOrUpdate(packet.MessageId, packet.Message, (id, p) => packet.Message);
                        //return;
                        break;
                }

                _droneTelemetry.Messages.AddOrUpdate(packet.MessageId, packet.Message, (id, p) => packet.Message);
                _eventAggregator.GetEvent<UpdateTelemetry>().Publish(_droneTelemetry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }                        
        }

        private T ExtractMessage<T>(MavLinkPacketBase msg) where T : UasMessage
        {
            return msg.Message as T;
        }
    }
}
