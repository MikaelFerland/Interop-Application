using Interop.Infrastructure.Events;

using Prism.Events;
using Prism.Mvvm;
using System;

namespace Interop.Modules.UserInterface.ViewModels
{
    public class SessionStatusViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public SessionStatusViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UpdateLoginStatusEvent>().Subscribe(Update_SessionStatus);
        }

        public void Update_SessionStatus(string sessionStatus)
        {
            this.SessionStatus = sessionStatus;
        }

        string _sessionStatus = string.Empty;
        public string SessionStatus
        {
            get
            {
                return this._sessionStatus;
            }
            set
            {
                if (SetProperty(ref _sessionStatus, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }
    }
}
