namespace UnitTests.AggregateRoot
{
    internal class AggregateTestBuilder
    {
        private TestRoot root;
        private bool callTestMethod;
        private bool verifyEventHandlerCalled;

        public AggregateTestBuilder GivenNoPreviousState()
        {
            root = new TestRoot();
            return this;
        }

        public AggregateTestBuilder GivenASimpleCommand()
        {
            callTestMethod = true;
            return this;
        }

        public AggregateTestBuilder CheckCodeInTheEventHandlerIsCalled()
        {
            verifyEventHandlerCalled = true;
            return this;
        }

        public AggregateTestBuilderExecutionPlan Build()
        {
            return new AggregateTestBuilderExecutionPlan
            {
                Root = root,
                CallTestMethod = callTestMethod,
                VerifyEventHandlerCalled = verifyEventHandlerCalled,
                CheckForEvents = CheckForEvents,
                CheckEventVersion = CheckForVersionNumber ? (long?)VersionNumberToCheck : null,
                CheckEventId = CheckEventId,
                BlankId = BlankId,
                CheckClearChanges = CheckClearChanges
            };
        }

        public AggregateTestBuilder CheckAnEventIsCreated()
        {
            CheckForEvents = true;
            return this;
        }

        public bool CheckForEvents { get; set; }

        public AggregateTestBuilder CheckTheEventHasAVersionNumberOf(long expectedVersionNumber)
        {
            CheckForVersionNumber = true;
            VersionNumberToCheck = expectedVersionNumber;
            return this;
        }

        public bool CheckForVersionNumber { get; set; }

        public long VersionNumberToCheck { get; set; }

        public AggregateTestBuilder CheckTheEventAggregateIdMatchesTheParentId()
        {
            CheckEventId = true;
            return this;
        }

        public bool CheckEventId { get; set; }

        public AggregateTestBuilder GivenAnEmptyId()
        {
            BlankId = true;
            return this;
        }

        public bool BlankId { get; set; }

        public AggregateTestBuilder CheckGetChangesIsEmptyAfterClearChanges()
        {
            CheckClearChanges = true;
            return this;
        }

        public bool CheckClearChanges { get; set; }
    }
}