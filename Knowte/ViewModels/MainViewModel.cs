using Knowte.Presentation.Contracts.Entities;
using Knowte.Services.Contracts.Collection;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Knowte.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private bool showCollections;
        private ObservableCollection<CollectionViewModel> collections;
        private ICollectionService collectionService;
        private CollectionViewModel selectedCollection;
        private CollectionViewModel activeCollection;

        public bool CanActivateSelectedCollection
        {
            get
            {
                if (this.selectedCollection == null)
                {
                    return false;
                }

                return !this.selectedCollection.Equals(this.activeCollection);
            }
        }

        public bool ShowCollections
        {
            get { return this.showCollections; }
            set
            {
                SetProperty<bool>(ref this.showCollections, value);
                RaisePropertyChanged(nameof(ContentIndex));
            }
        }
        public ObservableCollection<CollectionViewModel> Collections
        {
            get { return this.collections; }
            set { SetProperty<ObservableCollection<CollectionViewModel>>(ref this.collections, value); }
        }

        public CollectionViewModel SelectedCollection
        {
            get { return this.selectedCollection; }
            set
            {
                SetProperty<CollectionViewModel>(ref this.selectedCollection, value);
                RaisePropertyChanged(nameof(this.CanActivateSelectedCollection));
            }
        }

        public CollectionViewModel ActiveCollection
        {
            get { return this.activeCollection; }
            set
            {
                SetProperty<CollectionViewModel>(ref this.activeCollection, value);
                RaisePropertyChanged(nameof(this.CanActivateSelectedCollection));
            }
        }

        public int ContentIndex
        {
            get { return this.showCollections ? 0 : 1; }
        }

        public DelegateCommand PaneClosedCommand { get; set; }
        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand ActivateCollectionCommand { get; set; }

        public MainViewModel(ICollectionService collectionService)
        {
            this.collectionService = collectionService;

            this.PaneClosedCommand = new DelegateCommand(() => this.ShowCollections = false);
            this.LoadedCommand = new DelegateCommand(() => this.GetCollectionsAsync());
            this.ActivateCollectionCommand = new DelegateCommand(() => this.ActivateCollectionCommandHandler());
        }

        private void ActivateCollectionCommandHandler()
        {
            throw new NotImplementedException();
        }

        private async void GetCollectionsAsync()
        {
            // Remember the selected bank account
            string selectedCollectionId = string.Empty;

            if (this.SelectedCollection != null)
            {
                selectedCollectionId = this.SelectedCollection.Collection.Id;
            }

            this.Collections = new ObservableCollection<CollectionViewModel>(await this.collectionService.GetCollectionsAsync());

            this.ActiveCollection = null;
            this.SelectedCollection = null;
            this.ActiveCollection = this.Collections.Where(c => c.IsActive).FirstOrDefault();
            this.SelectedCollection = this.Collections.Where(c => c.Collection.Id.Equals(selectedCollectionId)).FirstOrDefault();
        }
    }
}
