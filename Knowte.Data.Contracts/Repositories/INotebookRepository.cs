using System.Threading.Tasks;

namespace Knowte.Data.Contracts.Repositories
{
    public interface INotebookRepository
    {
        Task<bool> DeleteNotebooks(string collectionId);
    }
}
