using Prism.Events;
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

namespace Interop.Modules.Obstacles.Views
{
    /// <summary>
    /// Interaction logic for ObstaclesView.xaml
    /// </summary>
    public partial class ObstaclesView : UserControl, Infrastructure.Interfaces.IView
    {
        public ObstaclesView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            DataContext = new ViewModels.ObstaclesViewModel(eventAggregator, this);
        }

    }
}