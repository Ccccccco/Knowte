using Knowte.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public class NotebookRepository : INotebookRepository
    {
        private ISQLiteConnectionFactory factory;

        public NotebookRepository(ISQLiteConnectionFactory factory)
        {
            this.factory = factory;
        }

        public async Task<string> AddNotebookAsync(string collectionId, string title)
        {
            string notebookId = string.Empty;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {

                    var notebook = new Notebook(collectionId, title);

                    conn.Insert(notebook);
                    notebookId = notebook.Id;
                }
            });

            return notebookId;
        }

        public async Task EditNotebookAsync(string notebookId, string title)
        {
            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    conn.Execute("UPDATE Notebook SET Title = ? WHERE Id=?;", title, notebookId, title);
                }
            });
        }

        public async Task<Notebook> GetNotebookAsync(string title)
        {
            Notebook notebook = null;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    notebook = conn.Table<Notebook>().Where((c) => c.Title.Equals(title)).FirstOrDefault();
                }
            });

            return notebook;
        }

        public async Task DeleteNotebookAsync(string notebookId)
        {
            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    conn.Execute("DELETE FROM Notebook WHERE Id = ?;", notebookId);
                }
            });
        }

        public async Task<List<Notebook>> GetNotebooksAsync(string collectionId)
        {
            var notebooks = new List<Notebook>();

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    notebooks = conn.Table<Notebook>().Select((c) => c).Where(c => c.CollectionId.Equals(collectionId)).ToList();

                }
            });

            return notebooks;
        }
    }
}
