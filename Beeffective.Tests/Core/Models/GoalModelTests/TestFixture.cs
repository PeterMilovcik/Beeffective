using Beeffective.Core.Models;
using NUnit.Framework;

namespace Beeffective.Tests.Core.Models.GoalModelTests
{
    public class TestFixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            SUT = new GoalModel();
        }

        protected GoalModel SUT { get; set; }
    }
}