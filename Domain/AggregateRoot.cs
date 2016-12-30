using System;
using System.Collections.Generic;
using System.Reflection;

namespace Domain
{
    public class AggregateRoot
    {
        private List<IAggregateEvent> changes = new List<IAggregateEvent>();

        public Guid Id { get; protected set; }

        protected void Apply(IAggregateEvent @event)
        {
            ValidateApply();
            AddToChanges(@event);
            CallHandler(@event);
        }

        private void ValidateApply()
        {
            if (this.Id == Guid.Empty)
            {
                throw new InvalidOperationException("Apply cannot be called unless the aggregate root has a valid Id");
            }
        }

        public void Restore(Guid id, List<IAggregateEvent> events)
        {
            this.Id = id;
            foreach (var @event in events)
            {
                this.Version = @event.Version;
                this.CallHandler(@event);
            }
        }

        private void AddToChanges(IAggregateEvent @event)
        {
            this.Version++;
            @event.AggregateId = this.Id;
            @event.Version = this.Version;
            this.changes.Add(@event);
        }

        public long Version { get; private set; }

        private void CallHandler(IAggregateEvent @event)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var methodToCall = this.GetType().GetMethod($"On{@event.GetType().Name}", bindingFlags);
            methodToCall.Invoke(this, bindingFlags, null,
                new object[] {@event}, null);
        }


        public IEnumerable<IAggregateEvent> GetChanges()
        {
            return this.changes;
        }

        public void ClearChanges()
        {
            this.changes.Clear();
        }
        
    }
}
