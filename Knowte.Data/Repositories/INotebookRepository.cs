using Knowte.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public interface INotebookRepository
    {
        Task<Notebook> GetNotebookAsync(string title);

        Task<string> AddNotebookAsync(string title);

        Task<bool> EditNotebookAsync(string notebookId, string title);

        Task<bool> DeleteNotebookAsync(string notebookId);

        Task<List<Notebook>> GetNotebooksAsync();
    }
}
