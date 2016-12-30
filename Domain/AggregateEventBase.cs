using System;

namespace Domain
{
    public abstract class AggregateEventBase : IAggregateEvent
    {
        public Guid AggregateId { get; set; }
        public long Version { get; set; }
        public string AggregateType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
