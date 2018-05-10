using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public interface INoteRepository
    {
        Task DeleteNotes(string notebookId);

        Task<IEnumerable<string>> GetAllNoteTitlesAsync();

        Task<string> AddNoteAsync(string notebookId, string title);
    }
}
