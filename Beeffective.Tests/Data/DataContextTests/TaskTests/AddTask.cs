using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.TaskTests
{
    class AddTask : TestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            CreateGoal();
            CreateProject(NewGoal);
            CreateTask(NewProject);
            SUT.Tasks.Add(NewTask);
            SUT.SaveChanges();
        }

        [Test]
        public void WithSpecifiedProperties() =>
            SUT.Tasks
                .SingleOrDefault(entity => 
                    entity.Title == NewTask.Title && 
                    entity.Description == NewTask.Description &&
                    entity.Project == NewTask.Project)
                .Should().NotBeNull();
    }
}