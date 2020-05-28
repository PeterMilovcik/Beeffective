using Beeffective.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Projects.NewProjectViewModelTests
{
    public class SaveProjectCommand : TestFixture
    {
        private const string ProjectTitle = "New Project Title";

        [SetUp]
        public void SetUp()
        {
            SUT.NewProject = null;
            SUT.Core.Projects.Clear();
        }

        [Test]
        public void CanExecute_NewProjectIsNull_False()
        {
            SUT.NewProject = null;
            SUT.SaveProjectCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewProjectTitleIsNull_False()
        {
            SUT.NewProject = new ProjectModel {Title = null};
            SUT.SaveProjectCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewProjectTitleIsEmpty_False()
        {
            SUT.NewProject = new ProjectModel { Title = string.Empty };
            SUT.SaveProjectCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewProjectTitleIsWhitespace_False()
        {
            SUT.NewProject = new ProjectModel { Title = " " };
            SUT.SaveProjectCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_GoalIsNull_False()
        {
            SUT.NewProject = new ProjectModel { Title = ProjectTitle, Goal = null };
            SUT.SaveProjectCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_SameProject_False()
        {
            SUT.Core.Projects.Add(new ProjectModel {Title = ProjectTitle, Goal = new GoalModel()});
            SUT.NewProject = new ProjectModel {Title = ProjectTitle, Goal = new GoalModel()};
            SUT.SaveProjectCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewProjectIsValid_True()
        {
            SUT.NewProject = new ProjectModel { Title = ProjectTitle, Goal = new GoalModel()};
            SUT.SaveProjectCommand.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void Execute_ProjectsContainsNewProject()
        {
            var newProject = new ProjectModel { Title = ProjectTitle, Goal = new GoalModel()};
            SUT.NewProject = newProject;
            SUT.SaveProjectCommand.Execute(null);
            SUT.Core.Projects.Should().Contain(newProject);
        }

        [Test]
        public void DialogDisplay_DialogIsClosed()
        {
            DialogDisplay.IsDialogShown = true;
            SUT.NewProject = new ProjectModel { Title = ProjectTitle, Goal = new GoalModel() };
            SUT.SaveProjectCommand.Execute(null);
            DialogDisplay.IsDialogShown.Should().BeFalse();
        }
    }
}