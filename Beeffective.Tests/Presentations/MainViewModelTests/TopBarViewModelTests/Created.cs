using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void Title_Beeffective() => 
            SUT.Title.Should().Be("Beeffective");

        [Test]
        public void NewGoal_NotNull() => 
            SUT.NewGoal.Should().NotBeNull();

        [Test]
        public void NewProject_NotNull() => 
            SUT.NewProject.Should().NotBeNull();
    }
}