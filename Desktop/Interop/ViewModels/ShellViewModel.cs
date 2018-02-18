using Prism.Mvvm;

namespace Interop.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private string _title = "Interop Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ShellViewModel()
        {

        }
    }
}
