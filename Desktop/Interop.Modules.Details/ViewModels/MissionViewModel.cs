using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Modules.Details.ViewModels
{
    class MissionViewModel : BindableBase
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

        public void Update_Mission(List<Mission> missions)
        {
            Missions = missions;
        }

        private IList<Mission> _missions;
        public IList<Mission> Missions
        {
            get
            {
                return _missions;
            }

            set
            {
                if (SetProperty(ref _missions, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        private Mission _mission;
        public Mission CurrentMission
        {
            get
            {
                return _mission;
            }
            set
            {
                if (SetProperty(ref _mission, value))
                {
                    GpsPoints = _mission.GetAllGpsPoints();
                    _eventAggregator.GetEvent<UpdateSelectedMission>().Publish(_mission);
                }
            }
        }

        List<BasePoint> _gpsPoints = new List<BasePoint>();
        public List<BasePoint> GpsPoints
        {
            get
            {
                return _gpsPoints;
            }
            set
            {
                if (SetProperty(ref _gpsPoints, value))
                {

                }
            }
        }

        private BasePoint _gpsPoint;
        public BasePoint GpsPoint
        {
            get
            {
                return _gpsPoint;
            }
            set
            {
                if(_gpsPoint != null)
                    _gpsPoint.IsSelected = false;

                if (SetProperty(ref _gpsPoint, value))
                {
                    _gpsPoint.IsSelected = true;
                }
            }
        }
    }
}
