using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interop.Infrastructure.Interfaces
{
    public interface IRequest
    {
        string Endpoint { get; }        
        eRequest Request { get; }
        object Data { get; set; }
        //TODO: Add an event
    }

    public enum eRequest : short
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 4
    };
}
