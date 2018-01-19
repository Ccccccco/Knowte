using Knowte.Data.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Data.Contracts.Repositories
{
    public interface ICollectionRepository
    {
        Task<List<Collection>> GetCollectionsAsync();

        Task<Collection> GetCollectionAsync(string title);

        Task<string> AddCollectionAsync(string title, bool isActive);
    }
}
