using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Prism.Events;
using System.Threading.Tasks;

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
                        Task<bool> waitTask;

                        _eventAggregator.GetEvent<SetTargetIdEvent>().Subscribe(delegate (int id)
                        {
                            response.Message = id.ToString();

                            Console.WriteLine(id.ToString());
                        });



                        waitTask = Task.Run(() =>
                        {
                            try
                            {
                                _eventAggregator.GetEvent<PostTargetEvent>().Publish(tInfo);
                                while (response.Message == null) ;

                                return true;
                            }
                            catch (AggregateException ae)
                            {
                                //Console.WriteLine("One or more exceptions occurred: ");
                                //foreach (var ex in ae.Flatten().InnerExceptions)
                                //    Console.WriteLine("   {0}", ex.Message);
                                return false;
                            }
                        });

                        waitTask.Wait();
                        //response.Message = "TARGET ADDED";
                        break;
                    }

                case InteropTargetMessage.OperationsTypes.DELETE:
                    {
                        _eventAggregator.GetEvent<DeleteTargetEvent>().Publish(tInfo.InteropID);
                        response.Message = "TARGET DELETED";
                        break;
                    }

                case InteropTargetMessage.OperationsTypes.EDIT:
                    {
                        _eventAggregator.GetEvent<PutTargetEvent>().Publish(tInfo);
                        response.Message = "TARGET EDITED";
                        break;
                    }
                default:
                    {
                        response.Message = "OPERATION NOT SUPPORTED";
                        break;
                    }                    
            }
            

            return response;            
        }
    }
}
