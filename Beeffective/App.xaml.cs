using System.ComponentModel.Composition.Hosting;
using System.Windows;

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

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //var mainViewModel = Container.GetExportedValue<MainViewModel>();
            //await mainViewModel.ShowAsync();
            //await mainViewModel.ChangeContentAsync(mainViewModel.Dashboard);
        }
    }
}
