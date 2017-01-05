using System;
using Bus;

namespace UnitTests.SubscriptionDispatcher
{
    internal class StartupSubscriber : IAmStartedByEventsOf<TestIncremented>
    {
        public void StartBecauseOf(TestIncremented @event)
        {
            throw new NotImplementedException();
        }
    }
}
