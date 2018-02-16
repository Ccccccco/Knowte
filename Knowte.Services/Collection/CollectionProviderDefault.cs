using Knowte.Data;
using Knowte.Data.Contracts.Entities;
using Knowte.Data.Contracts.Repositories;
using Knowte.Data.Repositories;
using Knowte.Plugin.Contracts.Collection;
using Knowte.Plugin.Contracts.Collection.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.Services.Collection
{
    [Export(typeof(ICollectionProvider))]
    public class CollectionProviderDefault : ICollectionProvider
    {
        private ICollectionRepository collectionRepository;
        private INotebookRepository notebookRepository;

        public string ProviderName => "[Default]";

        public CollectionProviderDefault()
        {
            this.collectionRepository = new CollectionRepository(new SQLiteConnectionFactory());
            this.notebookRepository = new NotebookRepository(new SQLiteConnectionFactory());
        }

        public async Task<bool> ActivateCollectionAsync(string collectionId)
        {
            return await this.collectionRepository.ActivateCollectionAsync(collectionId);
        }

        public async Task<string> AddCollectionAsync(string title, bool isActive)
        {
            return await this.collectionRepository.AddCollectionAsync(title, isActive);
        }

        public async Task<string> GetCollectionIdAsync(string title)
        {
            Data.Contracts.Entities.Collection collection = await this.collectionRepository.GetCollectionAsync(title);

            if( collection == null)
            {
                return string.Empty;
            }

            return collection.Id;
        }

        public async Task<bool> DeleteCollectionAsync(string collectionId)
        {
            return await this.collectionRepository.DeleteCollectionAsync(collectionId);
        }

        public async Task<bool> EditCollectionAsync(string collectionId, string title)
        {
            return await this.collectionRepository.EditCollectionAsync(collectionId, title);
        }

        public async Task<List<ICollection>> GetCollectionsAsync()
        {
            List<Data.Contracts.Entities.Collection> collections = await this.collectionRepository.GetCollectionsAsync();

            return collections.Cast<ICollection>().ToList();
        }

        public async Task<string> GetNotebookIdAsync(string title)
        {
            Notebook notebook = await this.notebookRepository.GetNotebookAsync(title);

            if (notebook == null)
            {
                return string.Empty;
            }

            return notebook.Id;
        }

        public async Task<string> AddNotebookAsync(string title)
        {
            return await this.notebookRepository.AddNotebookAsync(title);
        }

        public async Task<bool> EditNotebookAsync(string notebookId, string title)
        {
            return await this.notebookRepository.EditNotebookAsync(notebookId, title);
        }

        public async Task<bool> DeleteNotebookAsync(string notebookId)
        {
            return await this.notebookRepository.DeleteNotebookAsync(notebookId);
        }
    }
}
