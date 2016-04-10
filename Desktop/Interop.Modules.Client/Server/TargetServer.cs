using Interop.Infrastructure.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Prism.Events;

namespace Interop.Modules.Client.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TargetService" in both code and config file together.
    public class TargetServer : ITargetServer
    {
        //IEventAggregator _eventAggregator;
        //
        //public TargetServer(IEventAggregator eventAggregator)
        //{
        //    if (eventAggregator == null)
        //    {
        //        throw new ArgumentNullException("eventAggregator");
        //    }

        //    _eventAggregator = eventAggregator;
        //}

        public Response SendTarget(InteropTargetMessage tInfo)
        {
            //throw new NotImplementedException();
            var response = new Response();
            response.Message = "SUCCESS";
            Services.TargetService.CallbackTargetMessage(tInfo);

            return response;            
        }
    }
}
