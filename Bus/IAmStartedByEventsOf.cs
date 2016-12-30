using Domain;

namespace Bus
{
    public interface IAmStartedByEventsOf<in T> where T: IAggregateEvent
    {
        void StartBecauseOf(T @event);
    }
}
