using Knowte.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private ISQLiteConnectionFactory factory;

        public NoteRepository(ISQLiteConnectionFactory factory)
        {
            this.factory = factory;
        }

        public async Task DeleteNotes(string notebookId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAllNoteTitlesAsync()
        {
            IEnumerable<string> noteTitles = null;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    noteTitles = conn.Table<Note>().ToList().Select(n => n.Title).ToList();
                }
            });

            return noteTitles;
        }

        public async Task<string> AddNoteAsync(string notebookId, string title)
        {
            string noteId = string.Empty;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {

                    var note = new Note(notebookId, title);

                    conn.Insert(note);
                    noteId = note.Id;
                }
            });

            return noteId;
        }
    }
}
