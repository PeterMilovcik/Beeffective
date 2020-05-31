using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.Goals;
using DialogDisplay = Beeffective.Tests.Doubles.DialogDisplay;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.NewGoalViewModelTests
{
    public class TestFixture : Tests.TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            Core = Container.GetExportedValue<Presentation.Main.Core>();
            DialogDisplay = Container.GetExportedValue<IDialogDisplay>() as DialogDisplay;
            SUT = Core.Goals.New;
        }

        protected NewGoalViewModel SUT { get; set; }

        protected Presentation.Main.Core Core { get; set; }

        protected DialogDisplay DialogDisplay { get; set; }
    }
}