using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class AddCommand : TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            SUT.AddCommand.Execute(null);
        }

        [Test]
        public void IsAddMenuOpen_True() => SUT.IsAddMenuOpen.Should().BeTrue();
    }
}