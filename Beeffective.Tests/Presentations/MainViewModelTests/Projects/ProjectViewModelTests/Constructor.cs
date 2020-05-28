using System;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Projects.ProjectViewModelTests
{
    public class Constructor : TestFixture
    {
        [Test]
        public void ModelIsNull_ArgumentNullException()
        {
            Model = null;
            Assert.Throws<ArgumentNullException>(CreateSUT);
        }
    }
}