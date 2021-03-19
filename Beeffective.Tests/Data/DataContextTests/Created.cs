using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests
{
    class Created : TestFixture
    {
        [Test]
        public void NotNull() => SUT.Should().NotBeNull();
    }
}