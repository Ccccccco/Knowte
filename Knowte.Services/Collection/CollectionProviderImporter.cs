using Digimezzo.Foundation.Core.Settings;
using Knowte.WPF.PluginBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace Knowte.Services.Collection
{
    public class CollectionProviderImporter
    {
        public string SelectedProviderName => SettingsClient.Get<string>("CollectionProvider", "ProviderName");

        [ImportMany(typeof(ICollectionProvider))]
        private IEnumerable<Lazy<ICollectionProvider>> providers;

        public void DoImport()
        {
            // An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            // Add all the parts found in all assemblies in
            // the same directory as the executing program
            catalog.Catalogs.Add(
                new DirectoryCatalog(
                    Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location
                    )
                )
            );

            // Create the CompositionContainer with the parts in the catalog.
            CompositionContainer container = new CompositionContainer(catalog);

            // Fill the imports of this object
            container.ComposeParts(this);
        }

        public ICollectionProvider GetProvider()
        {
            foreach (Lazy<ICollectionProvider> provider in providers)
            {
                if (provider.Value.ProviderName.Equals(this.SelectedProviderName))
                {
                    return provider.Value;
                }
            }

            // If no provider was found, return the default provider.
            return new CollectionProviderDefault();
        }
    }
}
