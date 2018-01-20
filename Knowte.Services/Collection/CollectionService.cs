using System.Collections.Generic;
using System.Threading.Tasks;
using Knowte.Presentation.Contracts.Entities;
using Knowte.Services.Contracts.Collection;
using Knowte.Data.Contracts.Repositories;
using Digimezzo.Utilities.Log;
using System.Linq;
using System;

namespace Knowte.Services.Collection
{
    public class CollectionService : ICollectionService
    {
        private ICollectionRepository collectionRepository;

        public event CollectionChangedEventHandler CollectionAdded = delegate { };
        public event CollectionChangedEventHandler CollectionEdited = delegate { };
        public event CollectionChangedEventHandler CollectionDeleted = delegate { };
        public event CollectionChangedEventHandler ActiveCollectionChanged = delegate { };

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

        public async Task<ChangeCollectionResult> AddCollectionAsync(string title, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                LogClient.Error($"{nameof(title)} is empty");
                return ChangeCollectionResult.Invalid;
            }

            Data.Contracts.Entities.Collection existingCollection = await this.collectionRepository.GetCollectionAsync(title);

            if (existingCollection != null)
            {
                LogClient.Error($"There is already a collection with the title '{title}'");
                return ChangeCollectionResult.Duplicate;
            }

            string collectionId = await this.collectionRepository.AddCollectionAsync(title, isActive);

            if (string.IsNullOrEmpty(collectionId))
            {
                LogClient.Error($"{nameof(collectionId)} is empty");
                return ChangeCollectionResult.Error;
            }

            this.CollectionAdded(this, new CollectionChangedEventArgs(collectionId));
            LogClient.Info($"Added collection with title '{title}'");

            return ChangeCollectionResult.Ok;
        }

        public async Task<bool> ActivateCollectionAsync(CollectionViewModel collection)
        {
            if(collection == null)
            {
                LogClient.Error($"{nameof(collection)} is null");
                return false;
            }

            if (collection.Collection == null)
            {
                LogClient.Error($"{nameof(collection.Collection)} is null");
                return false;
            }

            if (string.IsNullOrEmpty(collection.Collection.Id))
            {
                LogClient.Error($"{nameof(collection.Collection.Id)} is null or empty");
                return false;
            }

            bool activateSuccess = await this.collectionRepository.ActivateCollection(collection.Collection.Id);

            if (!activateSuccess)
            {
                LogClient.Error($"Failed to activate collection with Id='{collection.Collection.Id}'");
                return false;
            }

            this.ActiveCollectionChanged(this, new CollectionChangedEventArgs(collection.Collection.Id));
            LogClient.Info($"Active collection changed. Active collection Id = '{collection.Collection.Id}'");

            return true;
        }
    }
}
