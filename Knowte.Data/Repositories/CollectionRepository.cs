using Digimezzo.Utilities.Log;
using Knowte.Data.Contracts;
using Knowte.Data.Contracts.Entities;
using Knowte.Data.Contracts.Repositories;
using System;
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
                try
                {
                    using (var conn = this.factory.GetConnection())
                    {
                        try
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
                        catch (Exception ex)
                        {
                            LogClient.Error("Could not add collection '{0}'. Exception: {1}", title, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error("Could not connect to the database. Exception: {0}", ex.Message);
                }
            });

            return collectionId;
        }

        public async Task<Collection> GetCollectionAsync(string title)
        {
            Collection collection = null;

            await Task.Run(() =>
            {
                try
                {
                    using (var conn = this.factory.GetConnection())
                    {
                        try
                        {
                            collection = conn.Table<Collection>().Where((c) => c.Title.Equals(title)).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            LogClient.Error("Could not get collection '{0}'. Exception: {1}", title, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error("Could not connect to the database. Exception: {0}", ex.Message);
                }
            });

            return collection;
        }

        public async Task<List<Collection>> GetCollectionsAsync()
        {
            var collections = new List<Collection>();

            await Task.Run(() =>
            {
                try
                {
                    using (var conn = this.factory.GetConnection())
                    {
                        try
                        {
                            collections = conn.Table<Collection>().Select((c) => c).ToList();
                        }
                        catch (Exception ex)
                        {
                            LogClient.Error("Could not get collections. Exception: {0}", ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error("Could not connect to the database. Exception: {0}", ex.Message);
                }
            });

            return collections;
        }
    }
}
