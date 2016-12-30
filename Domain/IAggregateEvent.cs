using System;

namespace Domain
{
    public interface IAggregateEvent
    {
        Guid AggregateId { get; set; }
        long Version { get; set; }
        string AggregateType { get; set; }
        DateTime Timestamp { get; set; }
    }
}
