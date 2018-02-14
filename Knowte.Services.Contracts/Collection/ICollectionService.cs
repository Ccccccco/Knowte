using Knowte.Presentation.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Services.Contracts.Collection
{
    public delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs e);
    public delegate void NotebookChangedEventHandler(object sender, NotebookChangedEventArgs e);

    public interface ICollectionService
    {
        event CollectionChangedEventHandler CollectionAdded;
        event CollectionChangedEventHandler CollectionEdited;
        event CollectionChangedEventHandler CollectionDeleted;
        event CollectionChangedEventHandler ActiveCollectionChanged;

        Task<List<CollectionViewModel>> GetCollectionsAsync();

        Task<ChangeCollectionResult> AddCollectionAsync(string title, bool isActive);

        Task<ChangeCollectionResult> EditCollectionAsync(CollectionViewModel collection, string title);

        Task<bool> ActivateCollectionAsync(CollectionViewModel collection);

        Task<bool> DeleteCollectionAsync(CollectionViewModel collection);
    }
}
