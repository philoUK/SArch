namespace UnitTests.SubscriptionDispatcher
{
    internal interface ITestSubscriber
    {
        bool TestMethodWasCalled { get; }
    }
}
