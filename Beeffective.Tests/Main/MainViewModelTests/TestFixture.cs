using Beeffective.Presentation.Main;

namespace Beeffective.Tests.Main.MainViewModelTests
{
    public class TestFixture : TestFixture<MainViewModel>
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainView = Container.GetExportedValue<IMainView>();
        }

        protected IMainView MainView { get; private set; }
    }
}