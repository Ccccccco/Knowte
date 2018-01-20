using Knowte.Data.Contracts.Repositories;
using System;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        public Task<bool> DeleteNotes(string notebookId)
        {
            throw new NotImplementedException();
        }
    }
}
