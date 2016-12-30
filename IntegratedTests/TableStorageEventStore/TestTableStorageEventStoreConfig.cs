using Persistence.Azure;

namespace IntegratedTests.TableStorageEventStore
{
    internal class TestTableStorageEventStoreConfig : ITableStorageEventStoreConfig
    {
        public string ConnectionString => "UseDevelopmentStorage=true";
        public string TableName => "IntegratedTestsEventStore";
    }
}
