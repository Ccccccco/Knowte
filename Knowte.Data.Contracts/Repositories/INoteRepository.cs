using System.Threading.Tasks;

namespace Knowte.Data.Contracts.Repositories
{
    public interface INoteRepository
    {
        Task<bool> DeleteNotes(string notebookId);
    }
}
