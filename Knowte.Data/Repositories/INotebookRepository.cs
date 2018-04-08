using Knowte.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public interface INotebookRepository
    {
        Task<Notebook> GetNotebookAsync(string title);

        Task<string> AddNotebookAsync(string collectionId, string title);

        Task EditNotebookAsync(string notebookId, string title);

        Task DeleteNotebookAsync(string notebookId);

        Task<List<Notebook>> GetNotebooksAsync(string collectionId);
    }
}
