using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Newtonsoft.Json;
using Prism.Events;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Services
{
    public class TelemetryService : ITelemetryService
    {
        BackgroundWorker _bw = new BackgroundWorker();
        UdpClient _udpClient;
        IEventAggregator _eventAggregator;

        DroneTelemetry _droneTelemetry = new DroneTelemetry();

        public TelemetryService(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;
            _droneTelemetry = new DroneTelemetry();

            Task.Run(async () =>
            {
                await ReceiveTelemetryAsync();
            });
        }

        private async Task ReceiveTelemetryAsync()
        {


            while (true)
            {
                try
                {
                    if (_udpClient == null)
                    {
                        _udpClient = new UdpClient();
                        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 14551);
                        _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        _udpClient.Client.Bind(RemoteIpEndPoint);
                    }

                    if (_udpClient.Client.IsBound)
                    {
                        // Blocks until a message returns on this socket from a remote host.
                        UdpReceiveResult results = await _udpClient.ReceiveAsync();
                        Byte[] receiveBytes = results.Buffer;
                        string mavlinkData = Encoding.ASCII.GetString(receiveBytes);

                        int mavlinkId = FindMavlinkId(mavlinkData);
                        RaiseMavlinkEvent(mavlinkId, mavlinkData);

                        // Uses the IPEndPoint object to determine which of these two hosts responded.
                        //Console.WriteLine("This is the message you received " +
                        //                             mavlinkData.ToString());
                        //Console.WriteLine("This message was sent from " +
                        //                            RemoteIpEndPoint.Address.ToString() +
                        //                            " on their port number " +
                        //                            RemoteIpEndPoint.Port.ToString());                    
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    _udpClient.Close();
                    _udpClient = null;
                }

            }
        }

        private void RaiseMavlinkEvent(int id, string mavlinkData)
        {
            try
            {
                switch (id)
                {
                    case 24: // GPS_RAW_INT
                        _droneTelemetry.GpsRawInt = JsonConvert.DeserializeObject<GpsRawInt>(mavlinkData);
                        break;
                    case 30: // ATTITUDE
                        _droneTelemetry.Attitude = JsonConvert.DeserializeObject<Attitude>(mavlinkData);
                        break;
                    case 33: // GLOBAL_POSITION_INT
                        _droneTelemetry.GlobalPositionInt = JsonConvert.DeserializeObject<GlobalPositionInt>(mavlinkData);
                        break;

                    case 74: // VFR_HUD
                        _droneTelemetry.VfrHUD = JsonConvert.DeserializeObject<VfrHUD>(mavlinkData);
                        break;

                    case 105: // HIGHRES_IMU
                        _droneTelemetry.HighresIMU = JsonConvert.DeserializeObject<HighresIMU>(mavlinkData);
                        break;

                    case 141: // ALTITUDE
                        _droneTelemetry.Altitude = JsonConvert.DeserializeObject<Altitude>(mavlinkData);
                        break;

                    default: // NOT YET SUPPORTED
                        return;
                }

                _eventAggregator.GetEvent<UpdateTelemetry>().Publish(_droneTelemetry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private int FindMavlinkId(string mavlinkStr)
        {
            string packetIdTag = "packet_id\":";

            int packetIndex = mavlinkStr.IndexOf(packetIdTag);
            if (packetIndex > 0)
            {
                // Remove to packet_id tag from the stream
                int endOfPacketIdTag = mavlinkStr.IndexOf(":", packetIndex);
                string packetIdTagEnd = mavlinkStr.Substring(packetIndex, endOfPacketIdTag - packetIndex);

                // Retrieve the packet id
                int beginOfPacketIdNum = endOfPacketIdTag + 1;
                int endOfPacketIdNum = mavlinkStr.IndexOf(",", packetIndex);

                // Parse the id
                int packetIdNumber; int.TryParse(mavlinkStr.Substring(beginOfPacketIdNum, endOfPacketIdNum - beginOfPacketIdNum), out packetIdNumber);

                return packetIdNumber;
            }

            return -1;
        }
    }
}