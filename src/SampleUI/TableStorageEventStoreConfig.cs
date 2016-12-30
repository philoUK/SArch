using Persistence.Azure;

namespace SampleUI
{
    public class TableStorageEventStoreConfig : ITableStorageEventStoreConfig
    {
        public string ConnectionString => "UseDevelopmentStorage=true";
        public string TableName => "SampleAppEventStore";
    }
}
