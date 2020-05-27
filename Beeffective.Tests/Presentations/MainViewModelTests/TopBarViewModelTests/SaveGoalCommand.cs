﻿using Beeffective.Core.Models;
using Beeffective.Presentation.Main.Goals;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class SaveGoalCommand : TestFixture
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
            SUT.NewGoal.Model.Title = null;
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void NewGoalTitleIsWhitespace_CanExecute_False()
        {
            SUT.NewGoal.Model.Title = " ";
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void NewGoalTitleIsNotWhitespace_CanExecute_True()
        {
            SUT.NewGoal.Model.Title = "New Goal Title";
            SUT.SaveGoalCommand.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void SameTitle_CanExecute_False()
        {
            var goalTitle = "New Goal Title";
            SUT.Tasks.Goals.Add(new GoalViewModel(new GoalModel {Title = goalTitle}));
            SUT.NewGoal.Model.Title = goalTitle;
            SUT.SaveGoalCommand.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void Execute_CloseDialog()
        {
            SUT.NewGoal.Model.Title = "New Goal Title";
            SUT.SaveGoalCommand.Execute(null);
            DialogDisplay.IsDialogShown.Should().BeFalse();
        }
    }
}