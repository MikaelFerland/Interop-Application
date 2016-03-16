using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Modules.Login.ViewModels
{
    public class SessionStatusViewModel : BindableBase
    {
        public SessionStatusViewModel()
        {
            Title = "Session Status Region";
        }

        public string Title { get; set; }
    }
}
