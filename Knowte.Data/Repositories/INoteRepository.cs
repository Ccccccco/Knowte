using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public interface INoteRepository
    {
        Task<bool> DeleteNotes(string notebookId);
    }
}
