using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

namespace Interop.Modules.Client.Requests
{
    class GetServerInfo : IRequest
    {

        public const string _endpoint = "api/server_info";
        public const eRequest _request = eRequest.GET;
        public ServerInfo _server_info = new ServerInfo();

        public object Data
        {
            get
            {
                return _server_info;
            }

            set
            {
                _server_info = value as ServerInfo;
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
