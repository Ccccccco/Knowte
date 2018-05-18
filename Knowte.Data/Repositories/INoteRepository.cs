using Knowte.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public interface INoteRepository
    {
        Task DeleteNotes(string notebookId);

        Task<List<string>> GetAllNoteTitlesAsync();

        Task<string> AddNoteAsync(string notebookId, string title);

        Task<List<Note>> GetNotesAsync(string notebookId);

        Task<List<Note>> GetAllNotesAsync();

        Task<List<Note>> GetUnfiledNotesAsync();

        Task DeleteNotesAsync(IList<string> noteIds);

        Task MarkNotesAsync(IList<string> noteIds);

        Task UnmarkNotesAsync(IList<string> noteIds);

        Task MoveNotesToNotebook(IList<string> noteIds, string notebookId);

        Task UnfileNotes(IList<string> noteIds);
    }
}
