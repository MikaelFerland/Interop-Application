using Interop.Infrastructure.Models;
using Prism.Events;

namespace Interop.Infrastructure.Events
{
    public class UpdateSelectedMission : PubSubEvent<Mission>
    {
    }
}
