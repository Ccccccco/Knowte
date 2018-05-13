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

        Task DeleteNoteAsync(string noteId);

        Task MarkNoteAsync(string noteId);

        Task UnmarkNoteAsync(string noteId);
    }
}
