using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Projects.NewProjectViewModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void NewProjectView_NotNull() => 
            SUT.NewProjectView.Should().NotBeNull();

        [Test]
        public void ShowNewProjectDialogCommand_NotNull() => 
            SUT.ShowNewProjectDialogCommand.Should().NotBeNull();

        [Test]
        public void Goals_NotNull() => 
            SUT.Core.Goals.Should().NotBeNull();

        [Test]
        public void SaveProjectCommand_NotNull() => 
            SUT.SaveProjectCommand.Should().NotBeNull();
    }
}