using Domain;

namespace Bus
{
    public interface IHandleSpecificEventsOf<in T> where T: IAggregateEvent
    {
        void HandleSpecificEvent(T @event);
    }
}
