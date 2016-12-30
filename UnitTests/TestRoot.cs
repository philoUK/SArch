using System;
using Domain;

namespace UnitTests
{
    internal class TestRoot : Domain.AggregateRoot
    {
        public void CallTestMethod(bool provideBlankId)
        {
            this.Id = provideBlankId ? Guid.Empty : Guid.NewGuid();
            Apply(new TestMethodCalled());
        }

        private void OnTestMethodCalled(TestMethodCalled @event)
        {
            this.TestMethodEventHandlerCalled = true;
        }

        public bool TestMethodEventHandlerCalled { get; set; }

        public void Increment()
        {
            Apply(new TestIncremented());
        }

        private void OnTestIncremented(TestIncremented @event)
        {
            IncrementCount++;
        }

        public int IncrementCount { get; private set; }
    }

    internal class TestMethodCalled : AggregateEventBase
    {
    }

    internal class TestIncremented : AggregateEventBase
    {
        
    }
}
