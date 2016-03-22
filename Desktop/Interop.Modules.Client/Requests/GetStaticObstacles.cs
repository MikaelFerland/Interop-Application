using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

namespace Interop.Modules.Client.Requests
{
    class GetStaticObstacles : IRequest
    {
        public const string _endpoint = "api/obstacles";
        public const eRequest _request = eRequest.GET;
        public List<stationary_obstacles> _staticObstacles = new List<stationary_obstacles>();

        public object Data
        {
            get
            {
                return _staticObstacles;
            }

            set
            {
                _staticObstacles = value as List<stationary_obstacles>;
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
