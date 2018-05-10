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

        Task ActivateCollectionAsync(string collectionId);

        Task DeleteCollectionAsync(string collectionId);

        Task EditCollectionAsync(string collectionId, string title);

        Task<string> GetActiveCollectionIdAsync();
    }
}
