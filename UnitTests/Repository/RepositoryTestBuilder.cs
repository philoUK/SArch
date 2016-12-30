using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Moq;
using Persistence;
using Xunit;

namespace UnitTests.Repository
{
    internal class RepositoryTestBuilder
    {
        private readonly Repository<TestRoot> target;
        private readonly Mock<IEventStore> eventStore;
        private TestRoot root;

        public RepositoryTestBuilder()
        {
            eventStore = new Mock<IEventStore>();
            target = new Repository<TestRoot>(eventStore.Object);
        }

        public RepositoryTestBuilder GivenNoEventsInEventStore()
        {
            eventStore.Setup(ies => ies.GetEventsFor(It.IsAny<Guid>())).Returns(Enumerable.Empty<IAggregateEvent>());
            return this;
        }

        public RepositoryTestBuilder WhenLoading()
        {
            this.root = target.Load(Guid.NewGuid());
            return this;
        }

        public RepositoryTestBuilder GivenEventStreamCountOf(int eventCount)
        {
            var results = new List<IAggregateEvent>();
            var id = Guid.NewGuid();
            for (var i = 0; i < eventCount; i++)
            {
                results.Add(new TestIncremented{AggregateId = id, Version = i+1});
            }
            eventStore.Setup(ies => ies.GetEventsFor(It.IsAny<Guid>())).Returns(results.AsEnumerable());
            return this;
        }

        public RepositoryTestBuilder WhenSavingAnEmptyAggregate()
        {
            root = new TestRoot();
            this.target.Save(new TestRoot());
            return this;
        }

        public RepositoryTestBuilder EventStoreIsNeverAskedToDoAnything()
        {
            this.eventStore.Verify(ies => ies.GetEventsFor(It.IsAny<Guid>()), Times.Never());
            return this;
        }

        public RepositoryTestBuilder WhenSavingAnAggregateWithChanges()
        {
            root = new TestRoot();
            root.CallTestMethod(false);
            this.target.Save(root);
            return this;
        }

        public RepositoryTestBuilder EventStoreIsAskedToSaveEvents()
        {
            this.eventStore.Verify(ies => ies.SaveEventsFor(It.Is<Guid>(arg => arg == this.root.Id), It.IsAny<IEnumerable<IAggregateEvent>>()), Times.Once());
            return this;
        }

        public RepositoryTestBuilder ReturnsNull()
        {
            Assert.Null(this.root);
            return this;
        }

        public RepositoryTestBuilder DoesNotReturnNull()
        {
            Assert.NotNull(this.root);
            return this;
        }

        public RepositoryTestBuilder CheckHasAVersionCountOf(int eventCount)
        {
            Assert.Equal(eventCount, this.root.Version);
            return this;
        }

        public RepositoryTestBuilder CheckHasNoNewPendingChanges()
        {
            Assert.Empty(this.root.GetChanges());
            return this;
        }
    }
}
