using UnitTests.BusDispatcher;
using Xunit;

namespace UnitTests.SubscriptionDispatcher
{
    public class Facts
    {
        [Fact]
        public void NormalSubscriberDispatchedToCorrectly()
        {
            new SubscriptionTestBuilder()
                .GivenATypeOf(typeof(ClassicSubscriber).AssemblyQualifiedName)
                .WhenDispatching()
                .ThenAppropriateDispatchMethodIsCalled();
        }
    }
}