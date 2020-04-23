using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Beeffective.Presentation.Main;

namespace Beeffective
{
    public partial class App
    {
        private CompositionContainer Container { get; }

        public App()
        {
            var catalog = new ApplicationCatalog();
            Container = new CompositionContainer(catalog);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainViewModel = Container.GetExportedValue<MainViewModel>();
            mainViewModel.Show();
        }
    }
}
