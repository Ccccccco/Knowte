using Knowte.Core.Prism;
using Knowte.Services.Collection;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Knowte.ViewModels.Notes
{
    public class NotesViewModel : BindableBase
    {
        private ICollectionService collectionService;
        private bool hasActiveCollection;
        private bool hasCollections;
        private bool isNotebookSelected;

        public DelegateCommand ManageCollectionsCommand { get; set; }

        public bool HasActiveCollection
        {
            get { return this.hasActiveCollection; }
            set { SetProperty<bool>(ref this.hasActiveCollection, value); }
        }

        public bool HasCollections
        {
            get { return this.hasCollections; }
            set { SetProperty<bool>(ref this.hasCollections, value); }
        }

        public NotesViewModel(ICollectionService collectionService, IEventAggregator eventAggregator)
        {
            this.collectionService = collectionService;

            this.EvaluateHasActiveCollectionAsync();
            this.EvaluateHasCollectionsAsync();

            this.collectionService.ActiveCollectionChanged += (_, e) => this.EvaluateHasActiveCollectionAsync();
            this.collectionService.CollectionAdded += (_, e) => this.EvaluateHasCollectionsAsync();
            this.collectionService.CollectionDeleted += (_, e) => {
                this.EvaluateHasActiveCollectionAsync();
                this.EvaluateHasCollectionsAsync();
                };

            this.ManageCollectionsCommand = new DelegateCommand(() => eventAggregator.GetEvent<ManageCollections>().Publish(null));
        }

        private async void EvaluateHasActiveCollectionAsync()
        {
            this.HasActiveCollection = await this.collectionService.HasActiveCollection();
        }

        private async void EvaluateHasCollectionsAsync()
        {
            this.HasCollections = (await this.collectionService.GetCollectionsAsync()).Count > 0;
        }
    }
}
