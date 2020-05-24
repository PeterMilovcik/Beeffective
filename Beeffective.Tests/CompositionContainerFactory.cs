using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Beeffective.Tests
{
    public class CompositionContainerFactory
    {
        public static CompositionContainer Create()
        {
            var directoryCatalog = new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory);

            var exportProvider = new CatalogExportProvider(directoryCatalog);

            var testCatalog = new AssemblyCatalog(Assembly.Load("Beeffective.Tests"));

            var container = new CompositionContainer(testCatalog, exportProvider);
            exportProvider.SourceProvider = container;
            return container;
        }
    }
}