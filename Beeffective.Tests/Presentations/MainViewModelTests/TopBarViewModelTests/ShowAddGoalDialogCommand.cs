using Beeffective.Presentation.Common;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class ShowAddGoalDialogCommand : TestFixture
    {
        private IMonitor<DelegateCommand> saveGoalCommandMonitor;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            saveGoalCommandMonitor = SUT.SaveGoalCommand.Monitor();
            SUT.ShowAddGoalDialogCommand.ExecuteAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void NewGoal_NotNull() => SUT.NewGoal.Should().NotBeNull();

        [Test]
        public void SaveGoalCommand_RaiseCanExecuteChanged() => 
            saveGoalCommandMonitor.Should().Raise("CanExecuteChanged");
    }
}