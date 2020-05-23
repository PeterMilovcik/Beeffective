using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Main.MainViewModelTests
{
    public class ShowAsync : TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            SUT.ShowAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void MainView_IsShown() => (MainView as Doubles.MainWindow)?.IsShown.Should().BeTrue();
    }
}