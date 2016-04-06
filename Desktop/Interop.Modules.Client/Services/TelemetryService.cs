using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interop.Modules.Client.Services
{
    public class TelemetryService : ITelemetryService
    {
        BackgroundWorker bw = new BackgroundWorker();
        IEventAggregator _eventAggregator;

        DroneTelemetry _droneTelemetry = new DroneTelemetry();

        //GpsRawInt _gpsRawInt;
        //Attitude _attitude;
        //GlobalPositionInt _globalPositionInt;
        //HighresIMU _highresIMU;
        //Altitude _altitude;
        
        public TelemetryService(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;
            _droneTelemetry = new DroneTelemetry();

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            bw.DoWork += Bw_DoWork;
            bw.RunWorkerAsync();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!(sender as BackgroundWorker).CancellationPending)
            {
                UdpClient udpClient = new UdpClient();
                try
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 5005);

                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    udpClient.Client.Bind(RemoteIpEndPoint);

                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
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

                    udpClient.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        private void RaiseMavlinkEvent(int id, string mavlinkData)
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

                case 105: // HIGHRES_IMU
                    _droneTelemetry.HighresIMU = JsonConvert.DeserializeObject<HighresIMU>(mavlinkData);
                    break;

                case 141: // ALTITUDE
                    _droneTelemetry.Altitude = JsonConvert.DeserializeObject<Altitude>(mavlinkData);
                    break;

                default : // NOT YET SUPPORTED
                    return;                    
            }

            _eventAggregator.GetEvent<UpdateTelemetry>().Publish(_droneTelemetry);
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
