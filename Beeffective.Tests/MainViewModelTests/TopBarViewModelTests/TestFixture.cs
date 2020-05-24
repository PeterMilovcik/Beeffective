using Beeffective.Data;
using Beeffective.Presentation.Main;
using Beeffective.Presentation.Main.TopBar;
using Repository = Beeffective.Tests.Doubles.Repository;

namespace Beeffective.Tests.MainViewModelTests.TopBarViewModelTests
{
    public class TestFixture : TestFixture<TopBarViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel = Container.GetExportedValue<MainViewModel>();
            Repository = Container.GetExportedValue<IRepository>() as Repository;
        }

        protected MainViewModel MainViewModel { get; private set; }
        protected Repository Repository { get; private set; }
    }
}