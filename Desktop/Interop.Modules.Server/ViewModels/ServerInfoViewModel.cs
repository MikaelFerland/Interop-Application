using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Modules.Server.ViewModels
{
    public class ServerInfoViewModel : BindableBase
    {
        public ServerInfoViewModel()
        {
            Title = "Server Info Region";
        }

        public string Title { get; set; }
    }
}
