using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

namespace Interop.Modules.Client.Requests
{
    public class GetObstacles : IRequest
    {
        public const string _endpoint = "api/obstacles";
        public const eRequest _request = eRequest.GET;
        public Obstacles _obstacles;

        public object Data
        {
            get
            {
                return _obstacles;
            }

            set
            {
                _obstacles = value as Obstacles;
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
