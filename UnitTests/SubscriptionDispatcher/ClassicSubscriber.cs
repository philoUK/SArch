using System;
using Bus;
using UnitTests.SubscriptionDispatcher;

namespace UnitTests.BusDispatcher
{
    internal class ClassicSubscriber : IHandleEventsOf<TestIncremented>, ITestSubscriber
    {
        public void HandleEvent(TestIncremented @event)
        {
            this.TestMethodWasCalled = true;
        }

        public bool TestMethodWasCalled { get; private set; }
    }
}
