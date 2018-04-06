using Knowte.Services.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Services.Collection
{
    public delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs e);
    public delegate void NotebookChangedEventHandler(object sender, NotebookChangedEventArgs e);

    public interface ICollectionService
    {
        event CollectionChangedEventHandler CollectionAdded;
        event CollectionChangedEventHandler CollectionEdited;
        event CollectionChangedEventHandler CollectionDeleted;
        event CollectionChangedEventHandler ActiveCollectionChanged;

        event NotebookChangedEventHandler NotebookAdded;
        event NotebookChangedEventHandler NotebookEdited;
        event NotebookChangedEventHandler NotebookDeleted;

        Task<List<CollectionViewModel>> GetCollectionsAsync();

        Task<ChangeCollectionResult> AddCollectionAsync(string title, bool isActive);

        Task<ChangeCollectionResult> EditCollectionAsync(CollectionViewModel collection, string title);

        Task<bool> ActivateCollectionAsync(CollectionViewModel collection);

        Task<bool> DeleteCollectionAsync(CollectionViewModel collection);

        Task<ChangeNotebookResult> AddNotebookAsync(string title);

        Task<ChangeNotebookResult> EditNotebookAsync(NotebookViewModel notebook, string title);

        Task<bool> DeleteNotebookAsync(NotebookViewModel notebook);

        Task<List<NotebookViewModel>> GetNotebooksAsync();
    }
}
