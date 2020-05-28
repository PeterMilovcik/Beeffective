using Beeffective.Core.Models;
using Beeffective.Presentation.Main.Projects;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Projects.ProjectViewModelTests
{
    public class TestFixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            Model = new ProjectModel();
            CreateSUT();
        }

        protected void CreateSUT() => SUT = new ProjectViewModel(Model);

        protected ProjectViewModel SUT { get; set; }

        protected ProjectModel Model { get; set; }
    }
}