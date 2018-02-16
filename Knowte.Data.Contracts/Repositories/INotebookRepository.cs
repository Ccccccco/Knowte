using Knowte.Data.Contracts.Entities;
using System.Threading.Tasks;

namespace Knowte.Data.Contracts.Repositories
{
    public interface INotebookRepository
    {
        Task<Notebook> GetNotebookAsync(string title);

        Task<string> AddNotebookAsync(string title);

        Task<bool> EditNotebookAsync(string notebookId, string title);

        Task<bool> DeleteNotebookAsync(string notebookId);
    }
}
