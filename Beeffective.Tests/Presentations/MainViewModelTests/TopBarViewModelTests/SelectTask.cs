using System.Linq;
using Beeffective.Data.Entities;
using Beeffective.Tests.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class SelectTask : TestFixture
    {
        private TaskEntity taskEntity;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            taskEntity = new TaskEntityBuilder().Create();
            Repository.Tasks.AddAsync(taskEntity).GetAwaiter().GetResult();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            SUT.Tasks.Selected = SUT.Tasks.First();
        }

        [Test]
        public void Title_SelectedTask() => SUT.Title?.Should().Be(taskEntity.Title);
    }
}