using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Core.Models.GoalModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void Id_Zero() => SUT.Id.Should().Be(0);

        [Test]
        public void Title_NullOrEmpty() => SUT.Title.Should().BeNullOrEmpty();

        [Test]
        public void Description_NullOrEmpty() => SUT.Description.Should().BeNullOrEmpty();
    }
}