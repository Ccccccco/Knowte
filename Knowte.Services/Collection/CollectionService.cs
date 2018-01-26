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
                LogClient.Error($"{nameof(collections)} is empty");
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
                LogClient.Error($"Add failed. {nameof(collectionId)} is empty");
                return ChangeCollectionResult.Error;
            }

            this.CollectionAdded(this, new CollectionChangedEventArgs(collectionId));
            LogClient.Info($"Add successful. {nameof(title)}={title}");

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

            bool activateSuccess = await this.collectionRepository.ActivateCollectionAsync(collection.Collection.Id);

            if (!activateSuccess)
            {
                LogClient.Error($"Activate failed. {nameof(collection.Collection.Id)}={collection.Collection.Id}");
                return false;
            }

            this.ActiveCollectionChanged(this, new CollectionChangedEventArgs(collection.Collection.Id));
            LogClient.Info($"Activate successful. {nameof(collection.Collection.Id)}={collection.Collection.Id}");

            return true;
        }

        public async Task<bool> DeleteCollectionAsync(CollectionViewModel collection)
        {
            if (collection == null)
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

            bool deleteSuccess = await this.collectionRepository.DeleteCollectionAsync(collection.Collection.Id);

            if (!deleteSuccess)
            {
                LogClient.Error($"Delete failed. {nameof(collection.Collection.Id)}={collection.Collection.Id}");
                return false;
            }

            this.CollectionDeleted(this, new CollectionChangedEventArgs(collection.Collection.Id));
            LogClient.Info($"Delete successful. {nameof(collection.Collection.Id)}={collection.Collection.Id}");

            return true;
        }

        public async Task<ChangeCollectionResult> EditCollectionAsync(CollectionViewModel collection, string title)
        {
            if (collection == null)
            {
                LogClient.Error($"{nameof(collection)} is null");
                return ChangeCollectionResult.Invalid;
            }

            if (collection.Collection == null)
            {
                LogClient.Error($"{nameof(collection.Collection)} is null");
                return ChangeCollectionResult.Invalid;
            }

            if (string.IsNullOrEmpty(collection.Collection.Id))
            {
                LogClient.Error($"{nameof(collection.Collection.Id)} is null or empty");
                return ChangeCollectionResult.Invalid;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                LogClient.Error($"{nameof(title)} is empty");
                return ChangeCollectionResult.Invalid;
            }

            Data.Contracts.Entities.Collection existingCollection = await this.collectionRepository.GetCollectionAsync(title);

            if (existingCollection != null)
            {
                LogClient.Error($"Collection with {nameof(title)}={title} already exists");
                return ChangeCollectionResult.Duplicate;
            }

            bool editSuccess = await this.collectionRepository.EditCollectionAsync(collection.Collection.Id, title);

            if (!editSuccess)
            {
                LogClient.Error($"Edit failed. {nameof(collection.Collection.Id)}={collection.Collection.Id}, {nameof(title)}={title}");
                return ChangeCollectionResult.Error;
            }

            this.CollectionEdited(this, new CollectionChangedEventArgs(collection.Collection.Id));
            LogClient.Info($"Edit success. {nameof(collection.Collection.Id)}={collection.Collection.Id}, {nameof(title)}={title}");

            return ChangeCollectionResult.Ok;
        }
    }
}
