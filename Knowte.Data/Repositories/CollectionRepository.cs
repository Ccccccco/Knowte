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
                            LogClient.Error($"Could not add collection. {nameof(title)}={title}. Exception: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error($"Could not connect to the database. Exception: {ex.Message}");
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
                            LogClient.Error($"Could not get collection. {nameof(title)}={title}. Exception: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error($"Could not connect to the database. Exception: {ex.Message}");
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
                            LogClient.Error($"Could not get collections. Exception: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error($"Could not connect to the database. Exception: {ex.Message}");
                }
            });

            return collections;
        }

        public async Task<bool> ActivateCollectionAsync(string collectionId)
        {
            bool isSuccess = false;

            await Task.Run(() =>
            {
                try
                {
                    using (var conn = this.factory.GetConnection())
                    {
                        try
                        {
                            conn.BeginTransaction();
                            conn.Execute("UPDATE Collection SET IsActive = 0;");
                            conn.Execute("UPDATE Collection SET IsActive = 1 WHERE Id=?;", collectionId);
                            conn.Commit();

                            isSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            LogClient.Error($"Could not activate the collection. {nameof(collectionId)}={collectionId}. Exception: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error($"Could not connect to the database. Exception: {ex.Message}");
                }
            });

            return isSuccess;
        }

        public async Task<bool> DeleteCollectionAsync(string collectionId)
        {
            bool isSuccess = false;

            await Task.Run(() =>
            {
                try
                {
                    using (var conn = this.factory.GetConnection())
                    {
                        try
                        {
                            conn.Execute("DELETE FROM Collection WHERE Id = ?;", collectionId);

                            isSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            LogClient.Error($"Could not delete the collection. {nameof(collectionId)}={collectionId}. Exception: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error($"Could not connect to the database. Exception: {ex.Message}");
                }
            });

            return isSuccess;
        }

        public async Task<bool> EditCollectionAsync(string collectionId, string title)
        {
            bool isSuccess = false;

            await Task.Run(() =>
            {
                try
                {
                    using (var conn = this.factory.GetConnection())
                    {
                        try
                        {
                            conn.Execute("UPDATE Collection SET Title = ? WHERE Id=?;", title, collectionId, title);

                            isSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            LogClient.Error($"Could activate the collection. {nameof(collectionId)}={collectionId}, {nameof(title)}={title}. Exception: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClient.Error($"Could not connect to the database. Exception: {ex.Message}");
                }
            });

            return isSuccess;
        }
    }
}
