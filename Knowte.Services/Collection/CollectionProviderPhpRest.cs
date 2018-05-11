using Knowte.PluginBase;
using Knowte.PluginBase.Collection.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Knowte.Services.Collection
{
    [Export(typeof(ICollectionProvider))]
    public class CollectionProviderPhpRest : ICollectionProvider
    {
        public string ProviderName => "PHP REST";

        public async Task ActivateCollectionAsync(string collectionId)
        {
            throw new System.NotImplementedException();
        }

      
        public async Task<string> GetCollectionIdAsync(string title)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteCollectionAsync(string collectionId)
        {
            throw new System.NotImplementedException();
        }

        public async Task EditCollectionAsync(string collectionId, string title)
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

        public async Task<string> GetNotebookIdAsync(string title)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> AddNotebookAsync(string collectionId, string title)
        {
            throw new System.NotImplementedException();
        }

        public async Task EditNotebookAsync(string notebookId, string title)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteNotebookAsync(string notebookId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<INotebook>> GetNotebooksAsync(string collectionId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetActiveCollectionId()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<string>> GetAllNoteTitlesAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> AddNoteAsync(string notebookId, string noteTitle)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<INote>> GetNotesAsync(string notebookId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<INote>> GetAllNotesAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<INote>> GetUnfiledNotesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
