using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Prism.Events;

namespace Interop.Infrastructure.Events
{
    public class PutTargetEvent : PubSubEvent<InteropTargetMessage>
    {
    }
}
