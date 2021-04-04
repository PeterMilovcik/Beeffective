using Beeffective.Data;
using Beeffective.Data.Entities;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests
{
    class TestFixture : TestFixture<DataContext>
    {
        protected GoalEntity AddedGoal { get; set; }
        protected GoalEntity NewGoal { get; set; }
        protected ProjectEntity NewProject { get; set; }
        protected TaskEntity NewTask { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
        }

        protected void CreateGoal()
        {
            NewGoal = new GoalEntity
            {
                Title = "Test Goal Title",
                Description = "Test Goal Description"
            };
        }

        protected void CreateProject(GoalEntity goal)
        {
            NewProject = new ProjectEntity
            {
                Title = "Test Project Title",
                Description = "Test Project Description",
                Goal = goal,
            };
        }

        private GoalEntity Save(GoalEntity entity)
        {
            var added = SUT.Goals.Add(entity).Entity;
            SUT.SaveChanges();
            return added;
        }

        protected void CreateTask(ProjectEntity project)
        {
            NewTask = new TaskEntity
            {
                Title = "Test Task Title",
                Description = "Test Task Description",
                Project = project,
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