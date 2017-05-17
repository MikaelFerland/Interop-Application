using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace Interop.Modules.Details.ViewModels
{
    class ObstaclesViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public ObstaclesViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<UpdateObstaclesEvent>().Subscribe(Update_Obstacles);
        }

        private string _title = "Obstacles";

        public string Title
        {
            get { return _title; }
        }

        public void Update_Obstacles(Obstacles obstacles)
        {
            Obstacles = obstacles;
        }

        Obstacles _obstacles;
        public Obstacles Obstacles
        {
            get
            {
                return _obstacles;
            }
            set
            {
                {
                    if (SetProperty(ref _obstacles, value))
                    {
                        //this.OnPropertyChanged(() => this.);
                    }
                }
            }
        }
    }
}
