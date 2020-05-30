using Beeffective.Presentation.Common;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.NewGoalViewModelTests
{
    public class Created : TestFixture
    {
        private IMonitor<AsyncCommand> saveGoalCommandMonitor;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            saveGoalCommandMonitor = SUT.SaveCommand.Monitor();
            SUT.ShowDialogCommand.ExecuteAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void SaveGoalCommand_RaiseCanExecuteChanged() =>
            saveGoalCommandMonitor.Should().Raise("CanExecuteChanged");

        [Test]
        public void ShowNewGoalDialogCommand_NotNull() => 
            SUT.ShowDialogCommand.Should().NotBeNull();

        [Test]
        public void NewGoal_NotNull() => 
            SUT.NewGoal.Should().NotBeNull();
    }
}