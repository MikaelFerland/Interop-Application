using Interop.Infrastructure.Interfaces;

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace Interop.Examples.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ITargetServer _targetServer;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            int id =99;
            int.TryParse(tbx_id.Text, out id);

            tbx_targetId.Text = id.ToString();
            cbx_operation.SelectedItem = InteropTargetMessage.OperationsTypes.NEW;
            cbx_operation.ItemsSource = Enum.GetValues(typeof(InteropTargetMessage.OperationsTypes));
            tbx_targetName.Text = "Test Target";
            tbx_targetId.Text = "999";
            tbx_posX.Text = "250";
            tbx_posY.Text = "500";

            tbx_latitude.Text = "45.5239885101173";
            tbx_longitude.Text = "73.4215310286758";
            tbx_orientation.Text = InteropTargetMessage.Orientations.N.ToString();
            tbx_shape.Text = InteropTargetMessage.Shapes.circle.ToString();
            tbx_character.Text = "r";
            tbx_backgroundColor.Text = InteropTargetMessage.Colors.black.ToString();
            tbx_foregroundColor.Text = InteropTargetMessage.Colors.blue.ToString();
            tbx_area.Text = "125000";

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
            interopMessage.InteropID = id;

            interopMessage.TargetID = id.ToString();
            interopMessage.Operation = (InteropTargetMessage.OperationsTypes) cbx_operation.SelectedItem;

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
            double.TryParse(tbx_latitude.Text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out latitude);
            interopMessage.Latitude = latitude;

            double longitude;
            double.TryParse(tbx_latitude.Text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out longitude);
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

            if (this._displayedImage != null)
            {
                interopMessage.Image = ImageSourceToBytes(new JpegBitmapEncoder(), _displayedImage);
            }

            _targetServer.SendTarget(interopMessage);
        }

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Image files (*.jpeg, *.png) | *.jpeg; *.png";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                ImagePath = openFileDialog1.FileName;
            }

            if (!string.IsNullOrEmpty(_imagePath))
            {
                var selectedImage = new BitmapImage(new Uri(_imagePath, UriKind.Absolute));

                _displayedImage?.Freeze(); // -> to prevent error: "Must create DependencySource on same Thread as the DependencyObject"
                DisplayedImage = selectedImage;
            }
            else
            {
                _displayedImage = null;
            }
        }

        ImageSource _displayedImage;
        public ImageSource DisplayedImage
        {
            get { return this._displayedImage; }
            set
            {
                if (value != this._displayedImage)
                {
                    this._displayedImage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        string _imagePath = string.Empty;
        public string ImagePath
        {
            get { return this._imagePath;}
            set
            {
                if (value != this._imagePath)
                {
                    this._imagePath = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // Source : http://stackoverflow.com/questions/29380416/convert-system-windows-media-imagesource-to-bytearray
        public byte[] ImageSourceToBytes(JpegBitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}