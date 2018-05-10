using Knowte.Data;
using Knowte.Data.Entities;
using Knowte.Data.Repositories;
using Knowte.PluginBase;
using Knowte.PluginBase.Collection.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.Services.Collection
{
    [Export(typeof(ICollectionProvider))]
    public class CollectionProviderDefault : ICollectionProvider
    {
        private ICollectionRepository collectionRepository;
        private INotebookRepository notebookRepository;
        private INoteRepository noteRepository;

        public string ProviderName => "[Default]";

        public CollectionProviderDefault()
        {
            this.collectionRepository = new CollectionRepository(new SQLiteConnectionFactory());
            this.notebookRepository = new NotebookRepository(new SQLiteConnectionFactory());
            this.noteRepository = new NoteRepository(new SQLiteConnectionFactory());
        }

        public async Task ActivateCollectionAsync(string collectionId)
        {
            await this.collectionRepository.ActivateCollectionAsync(collectionId);
        }

        public async Task<string> AddCollectionAsync(string title, bool isActive)
        {
            return await this.collectionRepository.AddCollectionAsync(title, isActive);
        }

        public async Task<string> GetCollectionIdAsync(string title)
        {
            Data.Entities.Collection collection = await this.collectionRepository.GetCollectionAsync(title);

            if( collection == null)
            {
                return string.Empty;
            }

            return collection.Id;
        }

        public async Task DeleteCollectionAsync(string collectionId)
        {
            await this.collectionRepository.DeleteCollectionAsync(collectionId);
        }

        public async Task EditCollectionAsync(string collectionId, string title)
        {
            await this.collectionRepository.EditCollectionAsync(collectionId, title);
        }

        public async Task<List<ICollection>> GetCollectionsAsync()
        {
            List<Data.Entities.Collection> collections = await this.collectionRepository.GetCollectionsAsync();

            return collections.Cast<ICollection>().ToList();
        }

        public async Task<string> GetNotebookIdAsync(string title)
        {
            Notebook notebook = await this.notebookRepository.GetNotebookAsync(title);

            if (notebook == null)
            {
                return string.Empty;
            }

            return notebook.Id;
        }

        public async Task<string> AddNotebookAsync(string collectionId, string title)
        {
            return await this.notebookRepository.AddNotebookAsync(collectionId, title);
        }

        public async Task EditNotebookAsync(string notebookId, string title)
        {
            await this.notebookRepository.EditNotebookAsync(notebookId, title);
        }

        public async Task DeleteNotebookAsync(string notebookId)
        {
            await this.notebookRepository.DeleteNotebookAsync(notebookId);
        }

        public async Task<List<INotebook>> GetNotebooksAsync(string collectionId)
        {
            List<Notebook> notebooks = await this.notebookRepository.GetNotebooksAsync(collectionId);

            return notebooks.Cast<INotebook>().ToList();
        }

        public async Task<string> GetActiveCollectionId()
        {
            return await this.collectionRepository.GetActiveCollectionIdAsync();
        }

        public async Task<IEnumerable<string>> GetAllNoteTitlesAsync()
        {
            return await this.noteRepository.GetAllNoteTitlesAsync();
        }

        public async Task<string> AddNoteAsync(string notebookId, string noteTitle)
        {
            return await this.noteRepository.AddNoteAsync(notebookId, noteTitle);
        }
    }
}
