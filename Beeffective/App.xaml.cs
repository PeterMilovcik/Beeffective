using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Beeffective.Presentation.Main;
using Syncfusion.Licensing;

namespace Beeffective
{
    public partial class App
    {
        private CompositionContainer Container { get; }

        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("##SyncfusionLicense##");
            var catalog = new ApplicationCatalog();
            Container = new CompositionContainer(catalog);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainViewModel = Container.GetExportedValue<MainViewModel>();
            mainViewModel.Show();
            await mainViewModel.Content.InitializeAsync();
        }
    }
}
