using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Goals;
using DialogDisplay = Beeffective.Tests.Doubles.DialogDisplay;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.NewGoalViewModelTests
{
    public class TestFixture : TestFixture<NewGoalViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            DialogDisplay = Container.GetExportedValue<IDialogDisplay>() as DialogDisplay;
        }

        protected DialogDisplay DialogDisplay { get; set; }
    }
}