using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void Core_NotNull() => SUT.Core.Should().NotBeNull();

        [Test]
        public void TopBar_NotNull() => SUT.TopBar.Should().NotBeNull();

        [Test]
        public void AlwaysOnTop_NotNull() => SUT.AlwaysOnTop.Should().NotBeNull();

        [Test]
        public void Calendar_NotNull() => SUT.Calendar.Should().NotBeNull();

        [Test]
        public void CalendarCommand_NotNull() => SUT.CalendarCommand.Should().NotBeNull();

        [Test]
        public void Dashboard_NotNull() => SUT.Dashboard.Should().NotBeNull();

        [Test]
        public void DashboardCommand_NotNull() => SUT.DashboardCommand.Should().NotBeNull();

        [Test]
        public void Settings_NotNull() => SUT.Settings.Should().NotBeNull();

        [Test]
        public void SettingsCommand_NotNull() => SUT.SettingsCommand.Should().NotBeNull();

        [Test]
        public void ContentViewModels_NotNull() => SUT.ContentViewModels.Should().NotBeNull();
    }
}