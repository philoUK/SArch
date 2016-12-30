using Microsoft.WindowsAzure.Storage.Table;

namespace Persistence.Azure
{
    public class AggregateIdentifier : TableEntity
    {
        public long Version { get; set; }
    }
}
