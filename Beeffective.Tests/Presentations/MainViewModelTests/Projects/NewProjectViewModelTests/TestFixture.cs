using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Projects;
using DialogDisplay = Beeffective.Tests.Doubles.DialogDisplay;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Projects.NewProjectViewModelTests
{
    public class TestFixture : TestFixture<NewProjectViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            DialogDisplay = Container.GetExportedValue<IDialogDisplay>() as DialogDisplay;
        }

        protected DialogDisplay DialogDisplay { get; set; }
    }
}