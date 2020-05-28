using Beeffective.Data.Repositories;
using Beeffective.Presentation.Main;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Presentation.Main.TopBar;
using Repository = Beeffective.Tests.Doubles.Repositories.Repository;
using DialogDisplay = Beeffective.Tests.Doubles.DialogDisplay;

namespace Beeffective.Tests.Presentations.MainViewModelTests.TopBarViewModelTests
{
    public class TestFixture : TestFixture<TopBarViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel = Container.GetExportedValue<MainViewModel>();
            Repository = Container.GetExportedValue<IRepository>() as Repository;
            DialogDisplay = Container.GetExportedValue<IDialogDisplay>() as DialogDisplay;
        }

        protected MainViewModel MainViewModel { get; private set; }
        protected Repository Repository { get; private set; }
        protected DialogDisplay DialogDisplay { get; private set; }
    }
}