using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interop.Modules.Telemetry.Controls
{
    /// <summary>
    /// Interaction logic for Led.xaml
    /// </summary>
    public partial class Led : UserControl
    {
        public Led()
        {
            InitializeComponent();
            if (this.IsActive)
                this.backgroundColor.Color = ColorOn;
            else
                this.backgroundColor.Color = ColorOff;
        }
        
        // Credit to http://mestaa.blogspot.ca/2011/04/led-for-wpf-how-to-create-wpf-user.html
        private static void IsActivePropertyChanced(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Led led = (Led)d;
            if ((bool)e.NewValue)
                led.backgroundColor.Color = led.ColorOn;
            else
                led.backgroundColor.Color = led.ColorOff;
        }

        private static void OnColorOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Led led = (Led)d;
            led.ColorOn = (Color)e.NewValue;
            if (led.IsActive)
                led.backgroundColor.Color = led.ColorOn;
            else
                led.backgroundColor.Color = led.ColorOff;
        }

        private static void OnColorOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Led led = (Led)d;
            led.ColorOff = (Color)e.NewValue;
            if (led.IsActive)
                led.backgroundColor.Color = led.ColorOn;
            else
                led.backgroundColor.Color = led.ColorOff;
        }

        /// <summary>Dependency property to Get/Set the current IsActive (True/False)</summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(Led),// null);
                new PropertyMetadata(new PropertyChangedCallback(Led.IsActivePropertyChanced)));

        /// <summary>Dependency property to Get/Set Color when IsActive is true</summary>
        public static readonly DependencyProperty ColorOnProperty =
            DependencyProperty.Register("ColorOn", typeof(Color), typeof(Led), //null);
                new PropertyMetadata(Colors.Green, new PropertyChangedCallback(Led.OnColorOnPropertyChanged)));

        /// <summary>Dependency property to Get/Set Color when IsActive is false</summary>
        public static readonly DependencyProperty ColorOffProperty =
            DependencyProperty.Register("ColorOff", typeof(Color), typeof(Led),// null);
                new PropertyMetadata(Colors.Red, new PropertyChangedCallback(Led.OnColorOffPropertyChanged)));

        /// <summary>Gets/Sets Value</summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        /// <summary>Gets/Sets Color when led is True</summary>
        public Color ColorOn
        {
            get
            {
                return (Color)GetValue(ColorOnProperty);
            }
            set
            {
                SetValue(ColorOnProperty, value);
            }
        }

        /// <summary>Gets/Sets Color when led is False</summary>
        public Color ColorOff
        {
            get
            {
                return (Color)GetValue(ColorOffProperty);
            }
            set
            {
                SetValue(ColorOffProperty, value);
            }
        }
    }
}
