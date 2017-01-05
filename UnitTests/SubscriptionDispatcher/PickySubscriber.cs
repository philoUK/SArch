using System;
using Bus;

namespace UnitTests.SubscriptionDispatcher
{
    internal class PickySubscriber : IHandleSpecificEventsOf<TestIncremented>
    {
        public void HandleSpecificEvent(TestIncremented @event)
        {
            throw new NotImplementedException();
        }
    }
}
