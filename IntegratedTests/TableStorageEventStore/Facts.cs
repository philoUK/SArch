using System;
using Xunit;

namespace IntegratedTests.TableStorageEventStore
{
    public class Facts
    {
        // simple empty list, check the table actually gets created in table storage
        [Fact]
        public void TableStorageTableActuallyGetsCreatedEvenWithEmptyEventStream()
        {
            new EventStoreTestBuilder()
                .GivenNoEventsToStream()
                .CheckTheUnderlyingAzureTableIsCreated();
        }

        // Simple stream of events can be written and read back
        [Fact]
        public void SaveAndLoadAStreamOfEvents()
        {
            var size = 1000;
            new EventStoreTestBuilder()
                .GivenAnEventStreamOfSize(size)
                .WhenWritingTheStream()
                .ThenTheCorrectNumberOfEventsShouldBeReadBack()
                .DeleteTheEventStream();
        }

        // Write a small stream, then try and write it again, should raise the appropriate error
        [Fact]
        public void WritingAnEventThatAlreadyHasARecordFailsAppropriately()
        {
            new EventStoreTestBuilder()
                .GivenAnEventStreamOfSize(5)
                .WhenWritingTheStream()
                .ThenFailsIfRepeated()
                .DeleteTheEventStream();
        }

    }
}
