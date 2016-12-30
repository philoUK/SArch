using System.Collections.Generic;
using Domain;

namespace Persistence
{
    public interface IEventPublisher
    {
        void PublishEvents(IEnumerable<IAggregateEvent> events);
    }
}