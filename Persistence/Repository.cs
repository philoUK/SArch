using System;
using System.Linq;
using Domain;

namespace Persistence
{
    public class Repository<T> : IRepository<T> where T: AggregateRoot
    {
        private readonly IEventStore eventStore;

        public Repository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }


        public T Load(Guid id)
        {
            var events = this.eventStore.GetEventsFor(id).OrderBy(e => e.Version).ToList();
            if (!events.Any())
            {
                return null;
            }
            var results = Activator.CreateInstance<T>();
            results.Restore(id, events);
            return results;
        }

        public void Save(T root)
        {
            var events = root.GetChanges().ToList();
            if (events.Any())
            {
                this.eventStore.SaveEventsFor(root.Id, events);
                root.ClearChanges();
            }
        }
    }
}
