using Domain;

namespace Bus
{
    public interface IHandleEventsOf<in T> where T:IAggregateEvent
    {
        void HandleEvent(T @event);
    }
}
