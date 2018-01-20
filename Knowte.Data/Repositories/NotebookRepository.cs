using Knowte.Data.Contracts.Repositories;
using System;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public class NotebookRepository : INotebookRepository
    {
        public Task<bool> DeleteNotebooks(string collectionId)
        {
            throw new NotImplementedException();
        }
    }
}
