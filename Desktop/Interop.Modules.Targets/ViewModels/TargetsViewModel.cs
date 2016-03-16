using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Modules.Targets.ViewModels
{
    public class TargetsViewModel : BindableBase
    {
        public TargetsViewModel()
        {
            Title = "Targets Region";
        }

        public string Title { get; set; }
    }
}
