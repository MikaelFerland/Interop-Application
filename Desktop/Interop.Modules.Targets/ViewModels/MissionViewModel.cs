using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Modules.Details.ViewModels
{
    class MissionViewModel
    {
        IEventAggregator _eventAggregator;

        public MissionViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<UpdateMissionEvent>().Subscribe(Update_Mission);
        }

        private string _title = "Missions";
        public string Title
        {
            get { return _title; }
        }

        private List<Mission> _missions;
        public void Update_Mission(List<Mission> missions)
        {
            _missions = missions;
        }
    }
}
