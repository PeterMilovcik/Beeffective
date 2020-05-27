using Beeffective.Core.Models;
using Beeffective.Presentation.Main.Goals;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.GoalViewModelTests
{
    public class TestFixture
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            GoalModel = new GoalModel();
            CreateSUT();
        }

        protected GoalModel GoalModel { get; set; }

        protected void CreateSUT() => SUT = new GoalViewModel(GoalModel);

        protected GoalViewModel SUT { get; set; }
    }
}