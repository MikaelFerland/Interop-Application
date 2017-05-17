using Interop.Infrastructure.Models;
using Prism.Events;
using System.Collections.Generic;

namespace Interop.Infrastructure.Events
{
    public class UpdateMissionEvent : PubSubEvent<List<Mission>>
    {
    }
}
