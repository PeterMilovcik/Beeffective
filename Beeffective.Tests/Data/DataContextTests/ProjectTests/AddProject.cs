using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.ProjectTests
{
    class AddProject : TestFixture
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            SUT.Projects.Add(NewProject);
            SUT.SaveChanges();
        }

        [Test]
        public void WithSpecifiedProperties() =>
            SUT.Projects
                .SingleOrDefault(
                    entity => entity.Title == NewProject.Title &&
                         entity.Description == NewProject.Description &&
                         entity.Goal == NewProject.Goal)
                .Should().NotBeNull();
    }
}
