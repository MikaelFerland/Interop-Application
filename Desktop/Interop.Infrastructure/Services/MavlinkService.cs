
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;
using Prism.Events;
using System;
using MavLinkNet;

namespace Interop.Infrastructure.Services
{
    public class MavlinkService : IMavlinkService
    {
        //private IEventAggregator _eventAggregator;
        //private MavLinkUdpTransport _mavlink;
        //private DroneTelemetry _droneTelemetry;

        //public MavlinkService(IEventAggregator eventAggregator)
        //{
        //    if (eventAggregator == null)
        //    {
        //        throw new ArgumentNullException("eventAggregator");
        //    }
        //    _eventAggregator = eventAggregator;

        //    _droneTelemetry = new DroneTelemetry();

        //    _mavlink = new MavLinkUdpTransport();
        //    _mavlink.TargetIpAddress = new System.Net.IPAddress(new byte[] { 0, 0, 0, 0 });
        //    _mavlink.UdpListeningPort = 14551;
        //    _mavlink.OnPacketReceived += Mavlink_OnPacketReceived;
        //    _mavlink.Initialize();
        //}

        ////TODO : Modify mavlink.net to be compatible with mavlink protocol v2
        //private void Mavlink_OnPacketReceived(object sender, MavLinkPacket packet)
        //{
        //    try
        //    {
        //        switch (packet.MessageId)
        //        {
        //            case 24: // GPS_RAW_INT
        //                _droneTelemetry.GpsRawInt = (UasGpsRawInt)packet.Message;
        //                break;
        //            case 30: // ATTITUDE
        //                _droneTelemetry.Attitude = (UasAttitude)packet.Message;
        //                break;
        //            case 33: // GLOBAL_POSITION_INT
        //                _droneTelemetry.GlobalPositionInt = (UasGlobalPositionInt)packet.Message;
        //                break;

        //            case 74: // VFR_HUD
        //                _droneTelemetry.VfrHUD = (UasVfrHud)packet.Message;
        //                break;

        //            case 105: // HIGHRES_IMU
        //                _droneTelemetry.HighresIMU = (UasHighresImu)packet.Message;
        //                break;

        //            case 141: // ALTITUDE
        //                _droneTelemetry.Altitude = (UasAltitude)packet.Message;
        //                break;

        //            default: // NOT YET SUPPORTED
        //                return;
        //        }

        //        _eventAggregator.GetEvent<UpdateTelemetry>().Publish(_droneTelemetry);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }                        
        //}
    }
}
