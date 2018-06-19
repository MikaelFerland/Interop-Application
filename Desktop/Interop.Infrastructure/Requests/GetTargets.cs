using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interop.Infrastructure.Models;

using Interop.Infrastructure.Interfaces;

namespace Interop.Modules.Client.Requests
{
    class GetTargets : IRequest
    {
        public const string _endpoint = "api/odlcs";
        public const eRequest _request = eRequest.GET;
        public List<Target> _targets = new List<Target>();

        public object Data
        {
            get
            {
                return _targets;
            }

            set
            {
                _targets = value as List<Target>;
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
