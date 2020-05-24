using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.MainViewModelTests.TopBarViewModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void Title_Beeffective() => SUT.Title.Should().Be("Beeffective");
    }
}