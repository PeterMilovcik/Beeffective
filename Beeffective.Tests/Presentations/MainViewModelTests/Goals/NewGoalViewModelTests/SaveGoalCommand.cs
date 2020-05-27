using System.Linq;
using Beeffective.Core.Models;
using Beeffective.Presentation.Main.Goals;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.NewGoalViewModelTests
{
    public class SaveGoalCommand : TestFixture
    {
        private const string NewGoalTitle = "New Goal Title";

        [SetUp]
        public void SetUp() => SUT.Tasks.Goals.Clear();

        [Test]
        public void CanExecute_NewGoalIsNull_False()
        {
            SUT.NewGoal = null;
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsNull_False()
        {
            SUT.NewGoal = new GoalViewModel(new GoalModel {Title = null});
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsEmpty_False()
        {
            SUT.NewGoal = new GoalViewModel(new GoalModel { Title = string.Empty });
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsWhitespace_False()
        {
            SUT.NewGoal = new GoalViewModel(new GoalModel { Title = " " });
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsValid_True()
        {
            SUT.NewGoal = new GoalViewModel(new GoalModel { Title = NewGoalTitle });
            SUT.SaveGoalCommand.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleAlreadyExist_False()
        {
            SUT.Tasks.Goals.Add(new GoalViewModel(new GoalModel{ Title = NewGoalTitle }));
            SUT.NewGoal = new GoalViewModel(new GoalModel { Title = NewGoalTitle });
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void Execute_DialogIsClosed()
        {
            SUT.NewGoal = new GoalViewModel(new GoalModel { Title = NewGoalTitle });
            SUT.SaveGoalCommand.Execute(null);
            DialogDisplay.IsDialogShown.Should().BeFalse();
        }

        [Test]
        public void Execute_GoalsContainNewGoal()
        {
            SUT.NewGoal = new GoalViewModel(new GoalModel { Title = NewGoalTitle });
            SUT.SaveGoalCommand.Execute(null);
            SUT.Tasks.Goals.SingleOrDefault(g => g.Model.Title == NewGoalTitle).Should().NotBeNull();
        }
    }
}