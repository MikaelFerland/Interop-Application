using Interop.Infrastructure.Models;
using Prism.Events;
using System.Collections.Generic;

namespace Interop.Infrastructure.Events
{
    public class UpdateTargetsEvent : PubSubEvent<List<Target>>
    {
    }
}
