using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.GoalTests
{
    class RemoveAddedGoal : TestFixture
    {
        public override void SetUp()
        {
            base.SetUp();
            CreateGoal();
            SUT.Goals.Add(NewGoal);
            SUT.SaveChanges();
            SUT.Goals.Remove(NewGoal);
            SUT.SaveChanges();
        }

        [Test]
        public void AddedGoalIsRemovedSuccessfully() =>
            SUT.Goals
                .SingleOrDefault(x => x.Title == NewGoal.Title && x.Description == NewGoal.Description)
                .Should().BeNull();
    }
}