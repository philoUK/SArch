using Xunit;

namespace UnitTests.AggregateRoot
{
    public class Facts
    {
        [Fact]
        public void AnAggregatesStateIsOnlyUpdatedViaTheApplyMethod()
        {
            GivenAnAggregateRootWithOneCommand()
                .CheckCodeInTheEventHandlerIsCalled()
                .Build()
                .Execute();
        }

        [Fact]
        public void AnAggregateProducesAnEventForASuccessfulMethodCall()
        {
            GivenAnAggregateRootWithOneCommand()
                .CheckAnEventIsCreated()
                .Build()
                .Execute();
        }

        [Fact]
        public void AnAggregateEventHasTheCorrectVersionNumber()
        {
            GivenAnAggregateRootWithOneCommand()
                .CheckTheEventHasAVersionNumberOf(1)
                .Build()
                .Execute();
        }

        [Fact]
        public void AnAggregateEventHasTheAggregateIdOfItsParent()
        {
            GivenAnAggregateRootWithOneCommand()
                .CheckTheEventAggregateIdMatchesTheParentId()
                .Build()
                .Execute();
        }

        [Fact]
        public void WithoutAnIdEventHandlingFails()
        {
            new AggregateTestBuilder()
                .GivenNoPreviousState()
                .GivenAnEmptyId()
                .GivenASimpleCommand()
                .Build()
                .CheckItFails();
        }

        [Fact]
        public void ClearingEventsProducesNoPendingChanges()
        {
            GivenAnAggregateRootWithOneCommand()
                .CheckGetChangesIsEmptyAfterClearChanges()
                .Build()
                .Execute();
        }

        private AggregateTestBuilder GivenAnAggregateRootWithOneCommand()
        {
            return new AggregateTestBuilder()
                .GivenNoPreviousState()
                .GivenASimpleCommand();
        }
    }
}
