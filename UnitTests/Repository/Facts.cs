using Xunit;

namespace UnitTests.Repository
{
    public class Facts
    {
        // test save, load, etc.
        [Fact]
        public void NoEventsInEventStoreLoadReturnsNull()
        {
            new RepositoryTestBuilder()
                .GivenNoEventsInEventStore()
                .WhenLoading()
                .ReturnsNull();
        }

        [Fact]
        public void EventsInEventStoreLoadAnAggregate()
        {
            var eventCount = 5;
            new RepositoryTestBuilder()
                .GivenEventStreamCountOf(eventCount)
                .WhenLoading()
                .DoesNotReturnNull()
                .CheckHasAVersionCountOf(eventCount);
        }

        [Fact]
        public void NoEventsDoesNotAskTheEventStoreToDoAnything()
        {
            new RepositoryTestBuilder()
                .WhenSavingAnEmptyAggregate()
                .EventStoreIsNeverAskedToDoAnything();
        }

        [Fact]
        public void SomeEventsDelegatesTheSavingToTheEventStore()
        {
            new RepositoryTestBuilder()
                .WhenSavingAnAggregateWithChanges()
                .EventStoreIsAskedToSaveEvents()
                .CheckHasNoNewPendingChanges();
        }
    }
}
