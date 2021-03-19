using System.Linq;
using Beeffective.Data.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.GoalTests
{
    class UpdateAddedGoal : TestFixture
    {
        private GoalEntity added;
        private const string UpdatedGoalTitle = "Updated Test Goal Title";

        public override void SetUp()
        {
            base.SetUp();
            added = SUT.Goals.Add(NewGoal).Entity;
            SUT.SaveChanges();
            added.Title = UpdatedGoalTitle;
            SUT.Goals.Update(added);
            SUT.SaveChanges();
        }

        [Test]
        public void UpdatedTitleIsSavedSuccessfully() =>
            SUT.Goals
                .SingleOrDefault(x => x.Title == UpdatedGoalTitle && x.Description == NewGoal.Description)
                .Should().NotBeNull();
    }
}