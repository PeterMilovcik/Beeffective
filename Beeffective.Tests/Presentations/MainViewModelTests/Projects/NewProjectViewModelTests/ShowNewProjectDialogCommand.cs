using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Projects.NewProjectViewModelTests
{
    public class ShowNewProjectDialogCommand : TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            SUT.ShowNewProjectDialogCommand.ExecuteAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void Dialog_IsShown() => 
            DialogDisplay.IsDialogShown.Should().BeTrue();

        [Test]
        public void NewProject_NotNull() => 
            SUT.NewProject.Should().NotBeNull();
    }
}