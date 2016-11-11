using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

namespace Interop.Modules.Client.Requests
{
    class GetDynamicObstacles : IRequest
    {
        public const string _endpoint = "api/obstacles";
        public const eRequest _request = eRequest.GET;
        public List<moving_obstacles> _dynamicObstacles = new List<moving_obstacles>();

        public object Data
        {
            get
            {
                return _dynamicObstacles;
            }

            set
            {
                _dynamicObstacles = value as List<moving_obstacles>;
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
