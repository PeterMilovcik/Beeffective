using Beeffective.Presentation.Main.Goals;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;

namespace Beeffective.Tests.MainViewModelTests.Goals.GoalViewModelTests
{
    public class ChangeTitle : TestFixture
    {
        private IMonitor<GoalViewModel> monitor;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            monitor = SUT.Monitor();
            SUT.Title = "New Title";
        }

        [Test]
        public void PropertyChanged() => monitor.Should().RaisePropertyChangeFor(p => p.Title);
    }
}