using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Modules.Obstacles.ViewModels
{
    public class ObstaclesViewModel : BindableBase
    {
        public ObstaclesViewModel()
        {
            Title = "Obstacles Region";
        }

        public string Title { get; set; }
    }
}
