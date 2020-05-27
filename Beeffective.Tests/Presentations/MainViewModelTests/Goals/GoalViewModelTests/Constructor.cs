using System;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.GoalViewModelTests
{
    public class Constructor : TestFixture
    {
        [Test]
        public void NullModel_ArgumentNullException()
        {
            GoalModel = null;
            Assert.Throws<ArgumentNullException>(CreateSUT);
        }
    }
}