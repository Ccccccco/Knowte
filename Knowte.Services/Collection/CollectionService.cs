using System.Collections.Generic;
using System.Threading.Tasks;
using Knowte.Presentation.Contracts.Entities;
using Knowte.Services.Contracts.Collection;
using Knowte.Data.Contracts.Repositories;
using Digimezzo.Utilities.Log;
using System.Linq;

namespace Knowte.Services.Collection
{
    public class CollectionService : ICollectionService
    {
        private ICollectionRepository collectionRepository;

        public CollectionService(ICollectionRepository collectionRepository)
        {
            this.collectionRepository = collectionRepository;
        }

        public async Task<List<CollectionViewModel>> GetCollectionsAsync()
        {
            List<Data.Contracts.Entities.Collection> collections = await this.collectionRepository.GetCollectionsAsync();

            if (collections.Count.Equals(0))
            {
                LogClient.Error("{0} is empty. Returning empty List<CollectionViewModel> list.", nameof(collections));
                return new List<CollectionViewModel>();
            }

            var collectionViewModels = new List<CollectionViewModel>();

            foreach (Data.Contracts.Entities.Collection collection in collections)
            {
                collectionViewModels.Add(new CollectionViewModel(collection));
            }

            return collectionViewModels.OrderBy(c => c.Collection.Title).ToList();
        }
    }
}
