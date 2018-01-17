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
