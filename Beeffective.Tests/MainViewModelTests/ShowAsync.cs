﻿using System.Linq;
using Beeffective.Data.Entities;
using Beeffective.Tests.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.MainViewModelTests
{
    public class ShowAsync : TestFixture
    {
        private TaskEntity taskEntity;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            taskEntity = new TaskEntityBuilder().Create();
            Repository.TaskEntities.Add(taskEntity);
            SUT.ShowAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void MainView_IsShown() => (MainView as Doubles.MainWindow)?.IsShown.Should().BeTrue();

        [Test]
        public void ContentViewModels_Contains_New() => SUT.ContentViewModels.Should().Contain(SUT.New);

        [Test]
        public void ContentViewModels_Contains_Dashboard() => SUT.ContentViewModels.Should().Contain(SUT.Dashboard);

        [Test]
        public void ContentViewModels_Contains_Priority() => SUT.ContentViewModels.Should().Contain(SUT.Priority);

        [Test]
        public void ContentViewModels_Contains_Goals() => SUT.ContentViewModels.Should().Contain(SUT.Goals);

        [Test]
        public void ContentViewModels_Contains_Tags() => SUT.ContentViewModels.Should().Contain(SUT.Tags);

        [Test]
        public void ContentViewModels_Contains_Calendar() => SUT.ContentViewModels.Should().Contain(SUT.Calendar);

        [Test]
        public void ContentViewModels_Contains_Settings() => SUT.ContentViewModels.Should().Contain(SUT.Settings);

        [Test]
        public void LoadTasks()
        {
            SUT.Tasks.Count.Should().Be(1);
            SUT.Tasks.First().Model.Title.Should().Be(taskEntity.Title);
        }
    }
}