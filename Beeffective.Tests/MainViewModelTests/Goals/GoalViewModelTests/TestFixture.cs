using Beeffective.Presentation.Main.Goals;
using NUnit.Framework;

namespace Beeffective.Tests.MainViewModelTests.Goals.GoalViewModelTests
{
    public class TestFixture
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            SUT = new GoalViewModel();
        }

        protected GoalViewModel SUT { get; set; }
    }
}