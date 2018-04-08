using Digimezzo.Foundation.Core.Logging;
using Digimezzo.Foundation.Core.Settings;
using Knowte.PluginBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Knowte.Services.Collection
{
    public class CollectionProviderImporter
    {
        private string pluginsDirectory;

        public string SelectedProviderName => SettingsClient.Get<string>("CollectionProvider", "ProviderName");

        [ImportMany(typeof(ICollectionProvider))]
        private IEnumerable<Lazy<ICollectionProvider>> providers;

        public CollectionProviderImporter(string pluginsFolder)
        {
            this.pluginsDirectory = pluginsFolder;
        }

        private void AddBuiltInParts(ref AggregateCatalog catalog)
        {
            // For builtin parts, we can safely assume that loading will succeed for all parts.
            // So use a DirectoryCatalog, which will scan all assemblies in the given directory.
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
        }

        private void AddCustomParts(string directory, ref AggregateCatalog catalog)
        {
            // Custom parts are written by users. If there is a mistake in the implementation,
            // or the implemented interface is out of date, loading of these parts can cause a
            // ReflectionTypeLoadException. When using a DirectoryCatalog, that exception causes
            // loading of all parts to fail. We don't want that. We want to load correct assemblies,
            // and prevent loading of wrong assemblies. So we'll use a AssemblyCatalog instead, 
            // which allows loading parts per dll, while testing if loading succeeds per dll.
            var directoryInfo = new DirectoryInfo(directory);

            if (!directoryInfo.Exists)
            {
                return;
            }

            var dllFileInfos = directoryInfo.GetFileSystemInfos("*.dll");

            foreach (var dllFileInfo in dllFileInfos)
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(Assembly.LoadFile(dllFileInfo.FullName));
                    var parts = assemblyCatalog.Parts.ToArray(); // throws ReflectionTypeLoadException 
                    catalog.Catalogs.Add(assemblyCatalog);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    LogClient.Error($"Could not load parts from assembly '{dllFileInfo.FullName}'. Exception: {ex.Message}");
                }
            }
        }

        public void DoImport()
        {
            // An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            // Add all the parts found in all assemblies in the same directory as the executing program
            this.AddBuiltInParts(ref catalog);

            // Add all the parts found in the Plugins directory
            if (!string.IsNullOrEmpty(this.pluginsDirectory))
            {
                this.AddCustomParts(this.pluginsDirectory, ref catalog);
            }

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
