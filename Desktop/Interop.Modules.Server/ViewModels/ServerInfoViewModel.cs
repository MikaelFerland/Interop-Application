using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Prism.Events;
using Prism.Mvvm;
using System;

namespace Interop.Modules.Server.ViewModels
{
    public class ServerInfoViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public ServerInfoViewModel(IEventAggregator eventAggregator ,IHttpService httpService)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;
            
            _eventAggregator.GetEvent<UpdateServerInfoEvent>().Subscribe(Update_ServerInfo);
        }

        public void Update_ServerInfo(ServerInfo serverInfo)
        {
            if (serverInfo != null)
            {
                this.ServerStatus = String.Format("{0} - {1}", serverInfo.server_time, serverInfo.message);
            }            
        }

        string _serverStatus = string.Empty;
        public string ServerStatus
        {
            get
            {
               return this._serverStatus;
            }
            set
            {
                if (SetProperty(ref _serverStatus, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        string _message = string.Empty;
        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                if (SetProperty(ref _message, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        string _messageTimeStamp = string.Empty;
        public string MessageTimeStamp
        {
            get
            {
                return this._messageTimeStamp;
            }
            set
            {
                if (SetProperty(ref _messageTimeStamp, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        string _serverTime = string.Empty;
        public string ServerTime
        {
            get
            {
                return this._serverTime;
            }
            set
            {
                if (SetProperty(ref _serverTime, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }
    }
}
