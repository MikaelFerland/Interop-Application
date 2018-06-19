using Interop.Infrastructure.Events;
using Interop.Infrastructure.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using Interop.Infrastructure;

namespace Interop.Modules.Details.ViewModels
{
    class MissionViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        public RelayCommand ExportMissionCommand { get; private set; }

        public MissionViewModel(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            this.ExportMissionCommand = new RelayCommand(this.ExportMission, t => this.CurrentMission != null);

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

        private void ExportMission()
        {
            var jsonToExport = QMission.CreateQGrouncontrolMission(CurrentMission);

            System.IO.File.WriteAllText("interop.mission", jsonToExport);
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
                    //this.ExportMissionCommand.RaiseCanExecuteChanged();
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
