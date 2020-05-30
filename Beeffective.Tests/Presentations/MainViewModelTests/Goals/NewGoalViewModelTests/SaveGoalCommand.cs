﻿using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.NewGoalViewModelTests
{
    public class SaveGoalCommand : TestFixture
    {
        private const string NewGoalTitle = "New Goal Title";

        [SetUp]
        public void SetUp()
        {
            SUT.NewGoal = null;
            SUT.Core.GoalsCollection.Clear();
        }

        [Test]
        public void CanExecute_NewGoalIsNull_False()
        {
            SUT.NewGoal = null;
            SUT.SaveCommand.CanExecute().Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsNull_False()
        {
            SUT.NewGoal = new GoalModel {Title = null};
            SUT.SaveCommand.CanExecute().Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsEmpty_False()
        {
            SUT.NewGoal = new GoalModel {Title = string.Empty};
            SUT.SaveCommand.CanExecute().Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsWhitespace_False()
        {
            SUT.NewGoal = new GoalModel {Title = " "};
            SUT.SaveCommand.CanExecute().Should().BeFalse();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleIsValid_True()
        {
            SUT.NewGoal = new GoalModel {Title = NewGoalTitle};
            SUT.SaveCommand.CanExecute().Should().BeTrue();
        }

        [Test]
        public void CanExecute_NewGoalModelTitleAlreadyExist_False()
        {
            SUT.Core.GoalsCollection.Add(new GoalModel {Title = NewGoalTitle});
            SUT.NewGoal = new GoalModel {Title = NewGoalTitle};
            SUT.SaveCommand.CanExecute().Should().BeFalse();
        }
    }
}