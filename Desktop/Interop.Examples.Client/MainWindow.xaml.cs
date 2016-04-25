using Interop.Infrastructure.Interfaces;

using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Windows;

namespace Interop.Examples.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ITargetServer _targetServer;

        public MainWindow()
        {
            InitializeComponent();
            RunClient();
        }

        void RunClient()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            string myIP = GetLocalIPAddress();

            Uri baseAddress = new Uri($"net.tcp://{myIP}:8000/targetserver");
            EndpointAddress address = new EndpointAddress(baseAddress);
            ChannelFactory<ITargetServer> channelFactory = new ChannelFactory<ITargetServer>(binding, address);
            _targetServer = channelFactory.CreateChannel();

            Console.ForegroundColor = ConsoleColor.Green;

        }

        // http://stackoverflow.com/questions/6803073/get-local-ip-address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InteropTargetMessage interopMessage = new InteropTargetMessage();

            int id;
            int.TryParse(tbx_id.Text, out id);

            interopMessage.TargetID = id.ToString();
            interopMessage.Operation = InteropTargetMessage.OperationsTypes.TEST;

            interopMessage.TargetName = tbx_targetName.Text;
            interopMessage.ImageSourceName = tbx_targetName.Text;
            interopMessage.TargetID = tbx_targetId.Text;

            int posX;
            int.TryParse(tbx_posX.Text, out posX);
            interopMessage.PosX = posX;

            int posY;
            int.TryParse(tbx_posY.Text, out posY);
            interopMessage.PosY = posY;

            double latitude;
            double.TryParse(tbx_latitude.Text, out latitude);
            interopMessage.Latitude = latitude;

            double longitude;
            double.TryParse(tbx_longitude.Text, out longitude);
            interopMessage.Longitude = longitude;

            int orientation;
            int.TryParse(tbx_orientation.Text, out orientation);
            interopMessage.Orientation = InteropTargetMessage.Orientations.N;

            interopMessage.Shape = InteropTargetMessage.Shapes.circle;
            interopMessage.Character = tbx_character.Text;
            interopMessage.BackgroundColor = InteropTargetMessage.Colors.black;
            interopMessage.ForegroundColor = InteropTargetMessage.Colors.blue;

            double area;
            double.TryParse(tbx_area.Text, out area);
            interopMessage.Area = area;

            _targetServer.SendTarget(interopMessage);
        }
    }
}