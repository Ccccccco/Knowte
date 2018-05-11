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

        public async Task<List<string>> GetAllNoteTitlesAsync()
        {
            List<string> noteTitles = null;

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

        public async Task<List<Note>> GetNotesAsync(string notebookId)
        {
            var notes = new List<Note>();

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    notes = conn.Table<Note>().Where(n => n.NotebookId.Equals(notebookId)).ToList();

                }
            });

            return notes;
        }

        public async Task<List<Note>> GetAllNotesAsync()
        {
            var notes = new List<Note>();

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    notes = conn.Table<Note>().ToList();

                }
            });

            return notes;
        }

        public async Task<List<Note>> GetUnfiledNotesAsync()
        {
            var notes = new List<Note>();

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    notes = conn.Query<Note>("SELECT * FROM Note WHERE NotebookId IS NULL OR NotebookId=''").ToList();
                }
            });

            return notes;
        }

        public async Task DeleteNoteAsync(string noteId)
        {
            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    conn.Execute("DELETE FROM Note WHERE Id = ?;", noteId);
                }
            });
        }
    }
}
