using GMap.NET.WindowsPresentation;
using Interop.Infrastructure.Interfaces;

using Prism.Events;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Interop.Modules.Map.Views
{
    /// <summary>
    /// Interaction logic for ObstaclesView.xaml
    /// </summary>
    public partial class MapView : UserControl, Infrastructure.Interfaces.IView
    {
        
        public MapView(IEventAggregator eventAggregator, IHttpService httpService)
        {
            InitializeComponent();
            this.Map.Zoom = 5;
            DataContext = new ViewModels.MapViewModel(eventAggregator, httpService, this);
        }
    }

    public class Map : GMapControl 
    { 
       public long ElapsedMilliseconds; 
 
       DateTime start; 
       DateTime end; 
       int delta; 
 

       private int counter; 
       readonly Typeface tf = new Typeface("GenericSansSerif"); 
       readonly System.Windows.FlowDirection fd = new System.Windows.FlowDirection(); 
 

       /// <summary> 
       /// any custom drawing here 
       /// </summary> 
       /// <param name="drawingContext"></param> 
       protected override void OnRender(DrawingContext drawingContext)
       { 
          start = DateTime.Now;
          base.OnRender(drawingContext);
          end = DateTime.Now; 
          delta = (int)(end - start).TotalMilliseconds; 
 
          FormattedText text = new FormattedText(string.Format(CultureInfo.InvariantCulture, "{0:0.0}", Zoom) + "z, " + MapProvider + ", refresh: " + counter++ + ", load: " + ElapsedMilliseconds + "ms, render: " + delta + "ms", CultureInfo.InvariantCulture, fd, tf, 10, Brushes.Lime); 
          drawingContext.DrawText(text, new Point(text.Height, text.Height)); 
          text = null; 
       }
    }
}