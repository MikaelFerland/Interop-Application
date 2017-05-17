using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

namespace Interop.Modules.Client.Requests
{
    public class GetMissions : IRequest
    {
        public const string _endpoint = "api/missions";
        public const eRequest _request = eRequest.GET;
        public List<Mission> _missions = new List<Mission>();

        public object Data
        {
            get
            {
                return _missions;
            }

            set
            {
                _missions = value as List<Mission>;
            }
        }

        string IRequest.Endpoint
        {
            get
            {
                return _endpoint;
            }
        }

        eRequest IRequest.Request
        {
            get
            {
                return _request;
            }
        }
    }
}
