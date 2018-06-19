using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

            this.ConnectCommand = new DelegateCommand(this.ConnectClient, this.CanConnectClient);

            Accounts = new List<Account> {
                new Account("10.10.130.10","ecole", "5861948105", "80"),
                //new Account("192.168.1.131","testadmin", "testpass", "8000"),
                //new Account("10.10.130.10","ets", "4532779881", "80")
            };

            CurrentAccount = Accounts[0];
        }

        public List<Account> Accounts { get; set; }

        private Account _currentAccount;
        public Account CurrentAccount
        {
            get => _currentAccount;

            set
            {
                if (SetProperty(ref _currentAccount, value))
                {
                    this.ConnectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _connected = false;

        private string _connectionStatus = "Connect";
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                if (SetProperty(ref _connectionStatus, value))
                {
                }
            }
        }

        private void ConnectClient()
        {
            if (_connected)
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

                Task.Factory.StartNew(async () =>
                {
                    _connected = await _httpService.Login(CurrentAccount.Username, CurrentAccount.Password, CurrentAccount.HostAddress, CurrentAccount.Port).ConfigureAwait(false);

                    if (_connected)
                    {
                        ConnectionStatus = "Disconnect";
                        _eventAggregator.GetEvent<UpdateLoginStatusEvent>().Publish(string.Format($"Connected"));

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
            public string HostAddress { get; }
            public string Username { get; }
            public string Password { get; }
            public string Port { get; }
            public string Name => $"{Username}@{HostAddress}";
        }
    }
}
