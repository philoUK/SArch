using System;
using System.Collections.Generic;
using Domain;

namespace Persistence
{
    public interface IEventStore
    {
        IEnumerable<IAggregateEvent> GetEventsFor(Guid aggregateRootId);
        void SaveEventsFor(Guid aggregateRootId, IEnumerable<IAggregateEvent> events);
    }
}