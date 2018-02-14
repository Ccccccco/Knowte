using Knowte.Data;
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

        public string ProviderName => "[Default]";

        public CollectionProviderDefault()
        {
            this.collectionRepository = new CollectionRepository(new SQLiteConnectionFactory());
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
    }
}
