using Beeffective.Data;
using Beeffective.Data.Entities;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests
{
    class TestFixture : TestFixture<DataContext>
    {
        protected GoalEntity NewGoal { get; set; }
        protected ProjectEntity NewProject { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            CreateGoal();
            CreateProject();
        }

        protected void CreateGoal()
        {
            NewGoal = new GoalEntity
            {
                Title = "Test Goal Title",
                Description = "Test Goal Description"
            };
        }

        protected void CreateProject()
        {
            var addedGoal = SUT.Goals.Add(NewGoal).Entity;
            NewProject = new ProjectEntity
            {
                Title = "Test Project Title",
                Description = "Test Project Description",
                Goal = addedGoal,
            };
        }

        [TearDown]
        public virtual void TearDown()
        {
            SUT.Goals.RemoveRange(SUT.Goals);
            SUT.Projects.RemoveRange(SUT.Projects);
            SUT.Labels.RemoveRange(SUT.Labels);
            SUT.TaskLabels.RemoveRange(SUT.TaskLabels);
            SUT.Tasks.RemoveRange(SUT.Tasks);
            SUT.SaveChanges();
        }
    }
}