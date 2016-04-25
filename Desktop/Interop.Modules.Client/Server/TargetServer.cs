using Interop.Infrastructure.Events;
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
        IEventAggregator _eventAggregator;

        public TargetServer()
        {
        }

        public TargetServer(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            _eventAggregator = eventAggregator;
        }

        public Response SendTarget(InteropTargetMessage tInfo)
        {
            //throw new NotImplementedException();
            var response = new Response();
            
            switch (tInfo.Operation)
            {
                case InteropTargetMessage.OperationsTypes.TEST:
                    {
                        response.Message = "PING OK";
                        break;
                    }
                case InteropTargetMessage.OperationsTypes.NEW:
                    {
                        _eventAggregator.GetEvent<SetTargetIdEvent>().Subscribe(delegate (int id)
                        {
                            response.Message = id.ToString();
                            Console.WriteLine(id.ToString());
                        });

                        _eventAggregator.GetEvent<PostTargetEvent>().Publish(tInfo);


                        break;
                    }
                default:
                    break;
            }
            
            return response;            
        }
    }
}
