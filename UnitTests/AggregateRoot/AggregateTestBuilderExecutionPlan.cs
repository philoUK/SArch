using System.Linq;
using Xunit;

namespace UnitTests.AggregateRoot
{
    internal class AggregateTestBuilderExecutionPlan
    {
        public bool CallTestMethod { get; set; }
        public TestRoot Root { get; set; }
        public bool VerifyEventHandlerCalled { get; set; }
        public bool CheckForEvents { get; set; }
        public long? CheckEventVersion { get; set; }
        public bool CheckEventId { get; set; }
        public bool BlankId { get; set; }
        public bool CheckClearChanges { get; set; }

        public void Execute()
        {
            SetUpRoot();
        }

        public void CheckItFails()
        {
            try
            {
                SetUpRoot();
                Assert.False(true, "Should have failed");
            }
            catch
            {
                // ignored
            }
        }

        private void SetUpRoot()
        {
            if (CallTestMethod)
            {
                this.Root.CallTestMethod(this.BlankId);
            }
            if (VerifyEventHandlerCalled)
            {
                Assert.True(this.Root.TestMethodEventHandlerCalled);
            }
            if (CheckForEvents)
            {
                Assert.NotEmpty(this.Root.GetChanges());
            }
            if (CheckEventVersion.HasValue)
            {
                var @event = this.Root.GetChanges().First();
                Assert.Equal(CheckEventVersion.Value, @event.Version);
            }
            if (CheckEventId)
            {
                var @event = this.Root.GetChanges().First();
                Assert.Equal(this.Root.Id, @event.AggregateId);
            }
            if (CheckClearChanges)
            {
                this.Root.ClearChanges();
                Assert.Empty(this.Root.GetChanges());
            }
        }
    }
}