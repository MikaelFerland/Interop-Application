using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

using Interop.Infrastructure.Interfaces;

namespace Interop.Modules.Server.ViewModels
{
    public class ServerInfoViewModel : BindableBase
    {
        IHttpService _httpService;

        public ServerInfoViewModel(IHttpService httpService)
        {
            _httpService = httpService;

            Title = "Server Info Region";
        }

        public string Title { get; set; }
    }
}
