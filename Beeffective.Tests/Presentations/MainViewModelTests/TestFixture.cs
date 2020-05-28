using Beeffective.Data.Repositories;
using Beeffective.Presentation.Main;
using Repository = Beeffective.Tests.Doubles.Repositories.Repository;

namespace Beeffective.Tests.Presentations.MainViewModelTests
{
    public class TestFixture : TestFixture<MainViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainView = Container.GetExportedValue<IMainView>();
            Repository = Container.GetExportedValue<IRepository>() as Repository;
        }

        protected IMainView MainView { get; private set; }

        protected Repository Repository { get; private set; }
    }
}