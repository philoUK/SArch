using System.Collections.Generic;
using Domain;

namespace Persistence
{
    internal class NullEventPublisher : IEventPublisher
    {
        public void PublishEvents(IEnumerable<IAggregateEvent> events)
        {
        }
    }
}
