using UnitTests.BusDispatcher;
using Xunit;

namespace UnitTests.SubscriptionDispatcher
{
    internal class SubscriptionTestBuilder
    {
        private string eventTypeName;
        private ITestSubscriber subscriber;
        public SubscriptionTestBuilder GivenATypeOf(string eventType)
        {
            this.eventTypeName = eventType;
            return this;
        }

        public SubscriptionTestBuilder WhenDispatching()
        {
            var dispatcher = new Bus.SubscriptionDispatcher(new TestSubscriberActivator());
            subscriber = (ITestSubscriber) dispatcher.Dispatch(this.eventTypeName, new TestIncremented());
            return this;
        }

        public SubscriptionTestBuilder ThenAppropriateDispatchMethodIsCalled()
        {
            Assert.True(this.subscriber.TestMethodWasCalled);
            return this;
        }
    }
}
