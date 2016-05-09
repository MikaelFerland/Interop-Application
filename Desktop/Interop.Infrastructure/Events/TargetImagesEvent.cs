using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Prism.Events;
using System.Collections.Concurrent;

namespace Interop.Infrastructure.Events
{
    public class TargetImagesEvent : PubSubEvent<ConcurrentDictionary<int, byte[]>>
    {
    }
}