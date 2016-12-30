using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Shared;

namespace Persistence.Azure
{
    public class TableStorageEventStore : IEventStore
    {
        private readonly ITableStorageEventStoreConfig config;
        private CloudTableClient tableClient;
        private CloudTable table;
        private const int MaxBatchSize = 100;
        private const string VersionRowkey = "CurrentVersion";

        public TableStorageEventStore(ITableStorageEventStoreConfig config)
        {
            this.config = config;
        }

        public IEnumerable<IAggregateEvent> GetEventsFor(Guid aggregateRootId)
        {
            this.InitialiseTable();
            var query = new TableQuery<AggregateEventData>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(TableConstants.PartitionKey, QueryComparisons.Equal, aggregateRootId.ToString()),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(TableConstants.RowKey, QueryComparisons.NotEqual, VersionRowkey)));
            foreach (var data in this.table.ExecuteQuery(query))
            {
                yield return AggregateEventExtensions.DeSerialise(data.EventData, data.EventType);
            }
        }

        public void SaveEventsFor(Guid aggregateRootId, IEnumerable<IAggregateEvent> events)
        {
            this.InitialiseTable();
            var remainingEvents = events.ToList();
            while (remainingEvents.Any())
            {
                if (remainingEvents.Count < MaxBatchSize)
                {
                    this.SubmitBatch(aggregateRootId, remainingEvents);
                    remainingEvents.Clear();
                }
                else
                {
                    var nextLot = remainingEvents.Take(MaxBatchSize - 1);
                    this.SubmitBatch(aggregateRootId, nextLot);
                    remainingEvents = remainingEvents.Skip(MaxBatchSize - 1).ToList();
                }
            }
        }

        private void SubmitBatch(Guid aggregateRootId, IEnumerable<IAggregateEvent> nextLot)
        {
            var data = nextLot.ToList();
            var version = data.OrderBy(e => e.Version).First().Version;
            var lastVersion = data.OrderByDescending(e => e.Version).First().Version;
            var expectedVersion = this.GetNextVersionFor(aggregateRootId);
            if (version != expectedVersion)
            {
                throw new EventStoreConcurrencyException($"Concurrency error writing events to event store for aggregate : {aggregateRootId}");
            }
            var batch = new TableBatchOperation();
            var header = new AggregateIdentifier
            {
                PartitionKey = aggregateRootId.ToString(),
                RowKey = VersionRowkey,
                Version = lastVersion
            };
            batch.InsertOrReplace(header);
            data.Select(this.ConvertToAggregateEventData).ToList().ForEach(item => batch.Insert(item));
            this.table.ExecuteBatch(batch);
        }

        private long GetNextVersionFor(Guid aggregateRootId)
        {
            this.InitialiseTable();
            var retrieveOp = TableOperation.Retrieve<AggregateIdentifier>(aggregateRootId.ToString(), VersionRowkey);
            var result = this.table.Execute(retrieveOp);
            if (result?.Result == null)
            {
                return 1L;
            }
            return ((AggregateIdentifier) result.Result).Version + 1;
        }

        private AggregateEventData ConvertToAggregateEventData(IAggregateEvent @event)
        {
            return new AggregateEventData
            {
                EventData = @event.ToJsonString(),
                EventType = @event.GetType().AssemblyQualifiedName,
                PartitionKey = @event.AggregateId.ToString(),
                RowKey = $"{@event.Version}".PadLeft(10, '0')
            };
        }

        private void InitialiseTable()
        {
            if (this.tableClient == null)
            {
                var storageAccount = CloudStorageAccount.Parse(this.config.ConnectionString);
                this.tableClient = storageAccount.CreateCloudTableClient();
                this.table = this.tableClient.GetTableReference(this.config.TableName);
                this.table.CreateIfNotExists();
            }
        }

        public void DeleteAggregate(Guid aggregateId)
        {
            this.InitialiseTable();
            var batch = new TableBatchOperation();
            var query = new TableQuery().Where(
                TableQuery.GenerateFilterCondition(TableConstants.PartitionKey, QueryComparisons.Equal, aggregateId.ToString()));
            foreach (var data in table.ExecuteQuery(query))
            {
                batch.Delete(data);
                if (batch.Count >= MaxBatchSize)
                {
                    table.ExecuteBatch(batch);
                    batch.Clear();
                }
            }
            if (batch.Count > 0)
            {
                table.ExecuteBatch(batch);
            }
        }
    }
}
