using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.WindowsAzure.Storage;
using Persistence;
using Xunit;

namespace IntegratedTests.TableStorageEventStore
{
    internal class EventStoreTestBuilder
    {
        private readonly Persistence.Azure.TableStorageEventStore eventStore;
        private readonly TestTableStorageEventStoreConfig config;
        private int streamLength;
        private List<TestTableStorageEvent> events;
        private Guid aggregateId;

        public EventStoreTestBuilder()
        {
            config = new TestTableStorageEventStoreConfig();
            eventStore = new Persistence.Azure.TableStorageEventStore(config);
        }

        public EventStoreTestBuilder GivenNoEventsToStream()
        {
            eventStore.SaveEventsFor(Guid.NewGuid(),Enumerable.Empty<IAggregateEvent>());
            return this;
        }

        public EventStoreTestBuilder CheckTheUnderlyingAzureTableIsCreated()
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(this.config.TableName);
            Assert.True(table.Exists());
            return this;
        }

        public EventStoreTestBuilder GivenAnEventStreamOfSize(int size)
        {
            this.aggregateId = Guid.NewGuid();
            this.streamLength = size;
            this.events = this.MakeEvents().ToList();
            return this;
        }

        private IEnumerable<TestTableStorageEvent> MakeEvents()
        {
            for (var i = 0; i < this.streamLength; i++)
            {
                yield return new TestTableStorageEvent
                {
                    AggregateId = this.aggregateId,
                    AggregateType = "test",
                    Timestamp = DateTime.UtcNow,
                    Version = i + 1
                };
            }
        }

        public EventStoreTestBuilder WhenWritingTheStream()
        {
            this.eventStore.SaveEventsFor(this.aggregateId, this.events.OrderBy(e => e.Version));
            return this;
        }

        public EventStoreTestBuilder ThenTheCorrectNumberOfEventsShouldBeReadBack()
        {
            var loadedEvents = this.eventStore.GetEventsFor(this.aggregateId);
            Assert.Equal(this.streamLength, loadedEvents.Count());
            return this;
        }

        public EventStoreTestBuilder DeleteTheEventStream()
        {
            this.eventStore.DeleteAggregate(this.aggregateId);
            return this;
        }

        public EventStoreTestBuilder ThenFailsIfRepeated()
        {
            try
            {
                this.eventStore.SaveEventsFor(this.aggregateId, this.events.OrderBy(e => e.Version));
                Assert.False(true, "Expected EventStoreConcurrencyException to be thrown");
            }
            catch (EventStoreConcurrencyException)
            {
                // Should pass fine and dandily
                return this;
            }
            catch (Exception ex)
            {
                Assert.False(true, "Unexpected exception");
            }
            return this;
        }
    }
}
