namespace Persistence.Azure
{
    public interface ITableStorageEventStoreConfig
    {
        string ConnectionString { get; }
        string TableName { get; }
    }
}