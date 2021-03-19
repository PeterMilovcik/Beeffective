using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.GoalTests
{
    class AddGoal : TestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            SUT.Goals.Add(NewGoal);
            SUT.SaveChanges();
        }

        [Test]
        public void GoalWithSpecifiedTitleAndDescriptionIsAdded() =>
            SUT.Goals
                .SingleOrDefault(x => x.Title == NewGoal.Title && x.Description == NewGoal.Description)
                .Should().NotBeNull();
    }
}
