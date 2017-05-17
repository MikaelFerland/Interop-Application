using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Interop.Infrastructure;
using Interop.Infrastructure.Interfaces;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Interop.Infrastructure.Events;

namespace Interop.Modules.UserInterface.ViewModels
{
    class MenuViewModel : BindableBase
    {
        IHttpService _httpService;
        IEventAggregator _eventAggregator;
        CancellationTokenSource _connectionCancellationSource;

        public DelegateCommand ConnectCommand { get; private set; }

        public MenuViewModel(IHttpService httpService, IEventAggregator eventAggregator)
        {
            if (httpService == null)
            {
                throw new ArgumentNullException("httpService");
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _httpService = httpService;
            _eventAggregator = eventAggregator;
                        
            this.ConnectCommand = new DelegateCommand(this.ConnectClient, this.CanConnectClient);
        }

        private string _username = "testuser";
        public string Username
        {
            get { return _username; }
            set
            {                
                if (SetProperty(ref _username, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        private string _password = "testpass";
        public string Password
        {
            get { return _password; }
            set
            {                
                if (SetProperty(ref _password, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        private bool _connected = false;

        private string _connectionStatus = "Connect";
        public string ConnectionStatus
        {
            get
            {
                return _connectionStatus;
            }
            set
            {

                if (SetProperty(ref _connectionStatus, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        private void ConnectClient()
        {
            if (_connected == true)
            {
                DisconnectClient();
            }
            else
            {
                _connectionCancellationSource = new CancellationTokenSource();
                _connectionCancellationSource.Token.Register(() =>
                {
                    _connected = false;
                    _eventAggregator.GetEvent<UpdateLoginStatusEvent>().Publish(string.Format($"Disconnected"));
                    ConnectionStatus = "Connect";
                });

                Task.Run(() => {
                    _connected = _httpService.Login(Username, Password);

                    if (_connected == true)
                    {
                        ConnectionStatus = "Disconnect";
                        _eventAggregator.GetEvent<UpdateLoginStatusEvent>().Publish(string.Format($"Connected as {Username}"));

                        while (!_connectionCancellationSource.Token.IsCancellationRequested)
                        {
                            _httpService.Run();
                        }
                    }
                    _connectionCancellationSource.Dispose();
                });
            }            
        }

        private void DisconnectClient()
        {
            _connectionCancellationSource.Cancel();
        }

        private bool CanConnectClient()
        {
            return true;
        }
    }
}
