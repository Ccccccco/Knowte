using Knowte.Plugin.Contracts.Collection;
using Knowte.Plugin.Contracts.Collection.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Knowte.Services.Collection
{
    [Export(typeof(ICollectionProvider))]
    public class CollectionProviderPhpRest : ICollectionProvider
    {
        public string ProviderName => "PHP REST";

        public async Task<bool> ActivateCollectionAsync(string collectionId)
        {
            throw new System.NotImplementedException();
        }

      
        public async Task<string> GetCollectionIdAsync(string title)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteCollectionAsync(string collectionId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> EditCollectionAsync(string collectionId, string title)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> AddCollectionAsync(string title, bool isActive)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<ICollection>> GetCollectionsAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
