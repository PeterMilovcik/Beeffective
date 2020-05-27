using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Goals;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;

namespace Beeffective.Tests.MainViewModelTests.TopBarViewModelTests
{
    public class ChangeNewGoalTitle : TestFixture
    {
        private IMonitor<DelegateCommand> monitor;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            SUT.ShowAddGoalDialogCommand.ExecuteAsync().GetAwaiter().GetResult();
            monitor = SUT.AddGoalCommand.Monitor();
            SUT.NewGoal.Title = "New Goal Title";
        }

        [Test]
        public void AddGoalCommand_CanExecuteChanged() => 
            monitor.Should().Raise("CanExecuteChanged");
    }
}