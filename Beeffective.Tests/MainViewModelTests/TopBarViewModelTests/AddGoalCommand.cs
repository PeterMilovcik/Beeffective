using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.MainViewModelTests.TopBarViewModelTests
{
    public class AddGoalCommand : TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            SUT.ShowAddGoalDialogCommand.ExecuteAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void NewGoalTitleIsNull_CanExecute_False()
        {
            SUT.NewGoal.Title = null;
            SUT.AddGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void NewGoalTitleIsWhitespace_CanExecute_False()
        {
            SUT.NewGoal.Title = " ";
            SUT.AddGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void NewGoalTitleIsNotWhitespace_CanExecute_True()
        {
            SUT.NewGoal.Title = "New Goal Title";
            SUT.AddGoalCommand.CanExecute(null).Should().BeTrue();
        }
    }
}