using Beeffective.Core.Models;
using FluentAssertions;
using FluentAssertions.Events;
using NUnit.Framework;

namespace Beeffective.Tests.Core.Models.GoalModelTests
{
    public class ChangeDescription : TestFixture
    {
        private IMonitor<GoalModel> monitor;

        public override void SetUp()
        {
            base.SetUp();
            monitor = SUT.Monitor();
            SUT.Description = "New Description";
        }

        [Test]
        public void PropertyChanged() => monitor.Should().RaisePropertyChangeFor(p => p.Description);
    }
}