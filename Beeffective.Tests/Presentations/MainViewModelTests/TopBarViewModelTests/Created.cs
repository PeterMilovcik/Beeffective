using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void Title_Beeffective() => 
            SUT.Title.Should().Be("Beeffective");

        [Test]
        public void ShowAddGoalDialogCommand_NotNull() => 
            SUT.ShowAddGoalDialogCommand.Should().NotBeNull();

        [Test]
        public void SaveGoalCommand_NotNull() =>
            SUT.SaveGoalCommand.Should().NotBeNull();
    }
}