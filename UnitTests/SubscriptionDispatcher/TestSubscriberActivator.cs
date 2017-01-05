using System;
using Bus;

namespace UnitTests.SubscriptionDispatcher
{
    internal class TestSubscriberActivator : ISubscriberActivator
    {
        public object CreateInstance(Type instanceType)
        {
            return Activator.CreateInstance(instanceType);
        }
    }
}
