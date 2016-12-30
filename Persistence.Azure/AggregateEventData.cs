using Microsoft.WindowsAzure.Storage.Table;

namespace Persistence.Azure
{
    public class AggregateEventData : TableEntity
    {
        public string EventType { get; set; }
        public string EventData { get; set; }
        public string AggregateType { get; set; }
        public bool IsRaised { get; set; }
    }
}
