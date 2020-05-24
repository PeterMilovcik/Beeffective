using Beeffective.Data;
using Beeffective.Presentation.Main.TopBar;
using Repository = Beeffective.Tests.Doubles.Repository;

namespace Beeffective.Tests.MainViewModelTests.TopBarViewModelTests
{
    public class TestFixture : TestFixture<TopBarViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            Repository = Container.GetExportedValue<IRepository>() as Repository;
        }

        public Repository Repository { get; private set; }
    }
}