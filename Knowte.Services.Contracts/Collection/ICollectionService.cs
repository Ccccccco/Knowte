using Knowte.Presentation.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Services.Contracts.Collection
{
    public delegate void CollectionAddedEventHandler(object sender, CollectionAddedEventArgs e);

    public interface ICollectionService
    {
        Task<List<CollectionViewModel>> GetCollectionsAsync();

        Task<AddCollectionResult> AddCollectionAsync(string title, bool isActive);

        event CollectionAddedEventHandler CollectionAdded;
    }
}
