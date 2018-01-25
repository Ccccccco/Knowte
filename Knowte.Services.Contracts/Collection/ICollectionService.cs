using Knowte.Presentation.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Services.Contracts.Collection
{
    public delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs e);

    public interface ICollectionService
    {
        Task<List<CollectionViewModel>> GetCollectionsAsync();

        Task<ChangeCollectionResult> AddCollectionAsync(string title, bool isActive);

        event CollectionChangedEventHandler CollectionAdded;
        event CollectionChangedEventHandler CollectionEdited;
        event CollectionChangedEventHandler CollectionDeleted;
        event CollectionChangedEventHandler ActiveCollectionChanged;

        Task<bool> ActivateCollectionAsync(CollectionViewModel collection);
        Task<bool> DeleteCollectionAsync(CollectionViewModel collection);
    }
}
