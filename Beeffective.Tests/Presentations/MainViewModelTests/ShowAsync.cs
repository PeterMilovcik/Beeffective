using Beeffective.Data.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests
{
    public class ShowAsync : TestFixture
    {
        private TaskEntity taskEntity;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            taskEntity = new TaskEntity {Id = 1, Title = "TestTask"};
            Repository.Tasks.AddAsync(taskEntity).GetAwaiter().GetResult();
            SUT.ShowAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void MainView_IsShown() => (MainView as Doubles.MainWindow)?.IsShown.Should().BeTrue();

        [Test]
        public void ContentViewModels_Contains_Dashboard() => SUT.ContentViewModels.Should().Contain(SUT.Dashboard);

        [Test]
        public void ContentViewModels_Contains_Goals() => SUT.ContentViewModels.Should().Contain(SUT.Goals);

        [Test]
        public void ContentViewModels_Contains_Calendar() => SUT.ContentViewModels.Should().Contain(SUT.Calendar);

        [Test]
        public void ContentViewModels_Contains_Settings() => SUT.ContentViewModels.Should().Contain(SUT.Settings);
    }
}