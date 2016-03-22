using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Events;
using Interop.Infrastructure.Models;

namespace Interop.Infrastructure.Events
{
    public class UpdateObstaclesEvent : PubSubEvent<Obstacles>
    {
    }
}
