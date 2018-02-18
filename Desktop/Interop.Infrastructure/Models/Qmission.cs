using System.Collections.Generic;

using Newtonsoft.Json;

using Interop.Infrastructure.Models;

namespace Interop.Infrastructure.Models
{
    public class QMission
    {
        [JsonProperty("firmwareType")]
        private int FirmwareType
        { 
            get { return 12; }            
        }

        [JsonProperty("groundStation")]
        private string GroundStation
        { 
            get { return "QGroundControl"; }            
        }

        [JsonProperty("items")]
        private List<SimpleItem> SimpleItems {get; set;}

        [JsonProperty("plannedHomePosition")]
        private double[] HomePosition { get; set;}

        [JsonProperty("version")]
        private int Version
        { 
            get { return 2; } 
        }

        public static string CreateQGrouncontrolMission(Mission mission)
        {
            QMission qmission = new QMission();
            qmission.SimpleItems = new List<SimpleItem>();

            if(mission.HomePosition != null)
            {
                qmission.HomePosition = new double[]{
                    mission.HomePosition.Latitude, 
                    mission.HomePosition.Longitude,
                    0 
                };

                qmission.SimpleItems.Add(new SimpleItem(
                    22, 
                    mission.HomePosition,
                    qmission.SimpleItems.Count+1));
            }

            if(mission.Waypoints != null)
            {
                foreach(Waypoint waypoint in mission.Waypoints)
                {
                qmission.SimpleItems.Add(new SimpleItem(
                    16,
                    waypoint,
                    qmission.SimpleItems.Count+1));
                }
            }
            
            return  JsonConvert.SerializeObject(qmission, Formatting.Indented);      
        }

        internal class SimpleItem
        {
            public SimpleItem (int command, BasePoint gpsPoint, int orderNum)
            {
                Command = command;

                this.Coordinate = new double[]{
                    gpsPoint.Latitude,
                    gpsPoint.Longitude,
                    0
                };
                
                DoJumpId = orderNum;
                
                Params = new int[]{0,0,0,0};
            }

            [JsonProperty("autoContinue")]
            public bool AutoContinue
            { 
                get { return true; }
            }

            [JsonProperty("command")]
            public int Command { get; set; }

            [JsonProperty("coordinate")]
            public double[] Coordinate { get; set; }

            [JsonProperty("doJumpId")]  
            public int DoJumpId { get; set; }

            [JsonProperty("frame")] 
            public int Frame
            { 
                get { return 3; }
            }

            [JsonProperty("params")] 
            public int[] Params { get; set; }

            [JsonProperty("type")]
            public string ItemType
            { 
                get { return "SimpleItem"; }
            }
        }
    }
}