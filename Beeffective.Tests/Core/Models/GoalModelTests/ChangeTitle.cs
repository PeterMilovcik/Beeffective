using Beeffective.Core.Models;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;

namespace Beeffective.Tests.Core.Models.GoalModelTests
{
    public class ChangeTitle : TestFixture
    {
        private IMonitor<GoalModel> monitor;

        public override void SetUp()
        {
            base.SetUp();
            monitor = SUT.Monitor();
            SUT.Title = "New Title";
        }

        [Test]
        public void PropertyChanged() => monitor.Should().RaisePropertyChangeFor(p => p.Title);
    }
}