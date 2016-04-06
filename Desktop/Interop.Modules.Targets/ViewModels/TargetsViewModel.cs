using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;

using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
             

            _eventAggregator.GetEvent<UpdateTargetsEvent>().Subscribe(Update_Targets);
        }

        public void Update_Targets(List<Target> targets)
        {
            this.Targets = new ObservableCollection<Target>(targets);
        }

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
