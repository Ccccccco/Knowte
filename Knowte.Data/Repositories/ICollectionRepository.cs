using Knowte.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public interface ICollectionRepository
    {
        Task<List<Collection>> GetCollectionsAsync();

        Task<Collection> GetCollectionAsync(string title);

        Task<string> AddCollectionAsync(string title, bool isActive);

        Task<bool> ActivateCollectionAsync(string collectionId);

        Task<bool> DeleteCollectionAsync(string collectionId);

        Task<bool> EditCollectionAsync(string collectionId, string title);
    }
}
