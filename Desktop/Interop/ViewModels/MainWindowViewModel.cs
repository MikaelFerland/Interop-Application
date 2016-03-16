using Prism.Mvvm;

namespace Interop.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Interop Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
