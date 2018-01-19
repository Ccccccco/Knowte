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

        public event CollectionAddedEventHandler CollectionAdded = delegate { };

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

        public async Task<AddCollectionResult> AddCollectionAsync(string title, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                LogClient.Error($"{nameof(title)} is empty");
                return AddCollectionResult.Invalid;
            }

            Data.Contracts.Entities.Collection existingCollection = await this.collectionRepository.GetCollectionAsync(title);

            if (existingCollection != null)
            {
                LogClient.Error($"There is already a collection with the title '{title}'");
                return AddCollectionResult.Duplicate;
            }

            string collectionId = await this.collectionRepository.AddCollectionAsync(title, isActive);

            if (string.IsNullOrEmpty(collectionId))
            {
                LogClient.Error($"{nameof(collectionId)} is empty");
                return AddCollectionResult.Error;
            }

            LogClient.Info($"Added collection with title '{title}'");
            this.CollectionAdded(this, new CollectionAddedEventArgs(collectionId, title));

            return AddCollectionResult.Ok;
        }
    }
}
