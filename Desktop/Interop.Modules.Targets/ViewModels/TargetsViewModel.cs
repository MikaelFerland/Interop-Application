using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;
using System.Collections.ObjectModel;

namespace Interop.Modules.Targets.ViewModels
{
    public class TargetsViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public TargetsViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            Title = "Targets Region";

            _eventAggregator.GetEvent<UpdateTargetsEvent>().Subscribe(Update_Targets);
        }

        public void Update_Targets(List<Target> targets)
        {
            this.Targets = new ObservableCollection<Target>(targets);
        }

        public string Title { get; set; }

        ObservableCollection<Target> _targets = new ObservableCollection<Target>(new List<Target>());
        public ObservableCollection<Target> Targets
        {
            get
            {
                return this._targets;
            }
            set
            {
                if (SetProperty(ref _targets, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }
    }
}
