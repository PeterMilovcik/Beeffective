using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class ShowAddGoalDialogCommand : TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            SUT.ShowAddGoalDialogCommand.ExecuteAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void NewGoal_NotNull() => SUT.NewGoal.Should().NotBeNull();
    }
}