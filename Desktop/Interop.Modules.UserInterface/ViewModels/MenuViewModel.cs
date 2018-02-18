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

            Accounts = new List<Account> {
                new Account("192.168.99.100","testuser", "testpass", "8000"),
                new Account("10.10.130.10","ets", "4532779881", "80")
            };

            CurrentAccount = Accounts[1];
        }

        public List<Account> Accounts { get; set; }

        private Account _currentAccount;
        public Account CurrentAccount
        {
            get
            {
                return _currentAccount;
            }

            set
            {
                if (SetProperty(ref _currentAccount, value))
                {
                    this.ConnectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _hostAddress = "192.168.99.100";
        public string HostAddress
        {
            get { return _hostAddress; }
            set
            {
                if (SetProperty(ref _hostAddress, value))
                {

                }
            }
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

                Task.Factory.StartNew(async () => {
                    _connected = await _httpService.Login(CurrentAccount.Username, CurrentAccount.Password, CurrentAccount.HostAddress, CurrentAccount.Port).ConfigureAwait(false);

                    if (_connected == true)
                    {
                        ConnectionStatus = "Disconnect";
                        _eventAggregator.GetEvent<UpdateLoginStatusEvent>().Publish(string.Format($"Connected as {Username}"));

                        await _httpService.Run(_connectionCancellationSource.Token).ConfigureAwait(false);

                    }
                    _connectionCancellationSource.Dispose();
                }, TaskCreationOptions.LongRunning);
            }            
        }

        private void DisconnectClient()
        {
            _connectionCancellationSource.Cancel();
        }

        private bool CanConnectClient()
        {
            return CurrentAccount != null;
        }

        internal class Account : BindableBase
        {
            public Account(string hostAddress, string username, string password, string port)
            {
                HostAddress = hostAddress;
                Username = username;
                Password = password;
                Port = port;
            }
            public string HostAddress { get; private set; }
            public string Username { get; private set; }
            public string Password { get; private set; }
            public string Port { get; private set; }
            public string Name
            {
                get
                {
                    return $"{Username}@{HostAddress}";
                }
            }
        }
    }
}
