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

        [Test]
        public void ContentViewModels_Contains_New() => SUT.ContentViewModels.Should().Contain(SUT.New);

        [Test]
        public void ContentViewModels_Contains_Dashboard() => SUT.ContentViewModels.Should().Contain(SUT.Dashboard);

        [Test]
        public void ContentViewModels_Contains_Priority() => SUT.ContentViewModels.Should().Contain(SUT.Priority);

        [Test]
        public void ContentViewModels_Contains_Goals() => SUT.ContentViewModels.Should().Contain(SUT.Goals);

        [Test]
        public void ContentViewModels_Contains_Tags() => SUT.ContentViewModels.Should().Contain(SUT.Tags);

        [Test]
        public void ContentViewModels_Contains_Calendar() => SUT.ContentViewModels.Should().Contain(SUT.Calendar);

        [Test]
        public void ContentViewModels_Contains_Settings() => SUT.ContentViewModels.Should().Contain(SUT.Settings);
    }
}