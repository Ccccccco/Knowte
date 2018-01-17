using Knowte.Presentation.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Services.Contracts.Collection
{
    public interface ICollectionService
    {
        Task<List<CollectionViewModel>> GetCollectionsAsync();
    }
}
