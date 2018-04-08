using Knowte.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.Data.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private ISQLiteConnectionFactory factory;

        public CollectionRepository(ISQLiteConnectionFactory factory)
        {
            this.factory = factory;
        }

        public async Task<string> AddCollectionAsync(string title, bool isActive)
        {
            string collectionId = string.Empty;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    if (isActive)
                    {
                        // If this collection should be active, make all other collections inactive.
                        conn.Execute("UPDATE Collection SET IsActive = 0;");
                    }

                    var collection = new Collection(title, isActive ? 1 : 0);

                    conn.Insert(collection);
                    collectionId = collection.Id;
                }
            });

            return collectionId;
        }

        public async Task<Collection> GetCollectionAsync(string title)
        {
            Collection collection = null;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    collection = conn.Table<Collection>().ToList().Where((c) => c.Title.Equals(title)).FirstOrDefault();
                }
            });

            return collection;
        }

        public async Task<List<Collection>> GetCollectionsAsync()
        {
            var collections = new List<Collection>();

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {

                    collections = conn.Table<Collection>().Select((c) => c).ToList();
                }
            });

            return collections;
        }

        public async Task ActivateCollectionAsync(string collectionId)
        {
            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    conn.BeginTransaction();
                    conn.Execute("UPDATE Collection SET IsActive = 0;");
                    conn.Execute("UPDATE Collection SET IsActive = 1 WHERE Id=?;", collectionId);
                    conn.Commit();
                }

            });
        }

        public async Task DeleteCollectionAsync(string collectionId)
        {
            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    conn.Execute("DELETE FROM Collection WHERE Id = ?;", collectionId);
                }
            });
        }

        public async Task EditCollectionAsync(string collectionId, string title)
        {
            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    conn.Execute("UPDATE Collection SET Title = ? WHERE Id=?;", title, collectionId, title);
                }
            });
        }

        public async Task<string> GetActiveCollectionId()
        {
            string activeCollectionId = string.Empty;

            await Task.Run(() =>
            {
                using (var conn = this.factory.GetConnection())
                {
                    activeCollectionId = conn.Table<Collection>().ToList().Where(c => c.IsActive == 1).Select(c => c.Id).FirstOrDefault();
                }
            });

            return activeCollectionId;
        }
    }
}
