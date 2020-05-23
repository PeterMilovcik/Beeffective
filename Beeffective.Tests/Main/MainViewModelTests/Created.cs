using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Main.MainViewModelTests
{
    public class Created : TestFixture
    {
        [Test]
        public void Tasks_NotNull() => SUT.Tasks.Should().NotBeNull();

        [Test]
        public void TopBar_NotNull() => SUT.TopBar.Should().NotBeNull();

        [Test]
        public void Priority_NotNull() => SUT.Priority.Should().NotBeNull();

        [Test]
        public void PriorityCommand_NotNull() => SUT.PriorityCommand.Should().NotBeNull();

        [Test]
        public void Tags_NotNull() => SUT.Tags.Should().NotBeNull();

        [Test]
        public void TagsCommand_NotNull() => SUT.TagsCommand.Should().NotBeNull();

        [Test]
        public void Goals_NotNull() => SUT.Goals.Should().NotBeNull();

        [Test]
        public void GoalsCommand_NotNull() => SUT.GoalsCommand.Should().NotBeNull();

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
        public void New_NotNull() => SUT.New.Should().NotBeNull();

        [Test]
        public void NewCommand_NotNull() => SUT.NewCommand.Should().NotBeNull();

        [Test]
        public void Settings_NotNull() => SUT.Settings.Should().NotBeNull();

        [Test]
        public void SettingsCommand_NotNull() => SUT.SettingsCommand.Should().NotBeNull();
    }
}