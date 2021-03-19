using System.Linq;
using Beeffective.Data.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.ProjectTests
{
    class UpdateAddedProject : TestFixture
    {
        private ProjectEntity added;
        private const string UpdatedProjectTitle = "Updated Test Project Title";

        public override void SetUp()
        {
            base.SetUp();
            added = SUT.Projects.Add(NewProject).Entity;
            SUT.SaveChanges();
            added.Title = UpdatedProjectTitle;
            SUT.Projects.Update(added);
            SUT.SaveChanges();
        }

        [Test]
        public void UpdatedTitleIsSavedSuccessfully() =>
            SUT.Projects
                .SingleOrDefault(entity =>
                    entity.Title == UpdatedProjectTitle &&
                    entity.Description == NewProject.Description &&
                    entity.Goal == NewProject.Goal)
                .Should().NotBeNull();
    }
}