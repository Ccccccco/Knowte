using Digimezzo.Utilities.Utils;
using Knowte.Presentation.Contracts.Entities;
using Knowte.Services.Constracts.Dialog;
using Knowte.Services.Contracts.Collection;
using Knowte.ViewModels.Dialogs;
using Knowte.Views.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private bool showCollections;
        private ObservableCollection<CollectionViewModel> collections;
        private IUnityContainer container;
        private ICollectionService collectionService;
        private IDialogService dialogService;
        private CollectionViewModel selectedCollection;
        private CollectionViewModel activeCollection;

        public bool CanActivateSelectedCollection
        {
            get
            {
                return this.selectedCollection != null ? !this.selectedCollection.Equals(this.activeCollection) : false;
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

        public int ContentIndex => this.showCollections ? 0 : 1;

        public DelegateCommand PaneClosedCommand { get; set; }

        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand ActivateCollectionCommand { get; set; }

        public DelegateCommand AddCollectionCommand { get; set; }

        public DelegateCommand EditCollectionCommand { get; set; }

        public DelegateCommand DeleteCollectionCommand { get; set; }

        public MainViewModel(IUnityContainer container, ICollectionService collectionService, IDialogService dialogService)
        {
            this.container = container;
            this.collectionService = collectionService;
            this.dialogService = dialogService;

            this.PaneClosedCommand = new DelegateCommand(() => this.ShowCollections = false);
            this.LoadedCommand = new DelegateCommand(() => this.GetCollectionsAsync());
            this.ActivateCollectionCommand = new DelegateCommand(async () => await this.ActivateCollectionCommandHandlerAsync());
            this.AddCollectionCommand = new DelegateCommand(() => this.AddCollectionCommandHandler());
            this.EditCollectionCommand = new DelegateCommand(() => this.EditCollectionCommandHandler());
            this.DeleteCollectionCommand = new DelegateCommand(async () => await this.DeleteCollectionCommandHandler());

            this.collectionService.CollectionAdded += (_, __) => this.GetCollectionsAsync();
            this.collectionService.CollectionEdited += (_, __) => this.GetCollectionsAsync();
            this.collectionService.CollectionDeleted += (_, __) => this.GetCollectionsAsync();
            this.collectionService.ActiveCollectionChanged += (_, __) => this.GetCollectionsAsync();
        }

        private async Task ActivateCollectionCommandHandlerAsync()
        {
            if (!await this.collectionService.ActivateCollectionAsync(this.selectedCollection))
            {
                this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Activate_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Activate_Collection"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
            }
        }

        private void AddCollectionCommandHandler()
        {
            AddCollection view = this.container.Resolve<AddCollection>();
            AddCollectionViewModel viewModel = this.container.Resolve<AddCollectionViewModel>();
            view.DataContext = viewModel;

            this.dialogService.ShowCustom(
                ResourceUtils.GetString("Language_Add_Collection"),
                view,
                420,
                0,
                false,
                true,
                true,
                true,
                ResourceUtils.GetString("Language_Ok"),
                ResourceUtils.GetString("Language_Cancel"),
                async () => await viewModel.AddCollectionAsync());
        }

        private void EditCollectionCommandHandler()
        {
            EditCollection view = this.container.Resolve<EditCollection>();
            EditCollectionViewModel viewModel = this.container.Resolve<EditCollectionViewModel>(new DependencyOverride(typeof(CollectionViewModel), this.SelectedCollection));
            view.DataContext = viewModel;

            this.dialogService.ShowCustom(
                ResourceUtils.GetString("Language_Edit_Collection"),
                view,
                420,
                0,
                false,
                true,
                true,
                true,
                ResourceUtils.GetString("Language_Ok"),
                ResourceUtils.GetString("Language_Cancel"),
                async () => await viewModel.EditCollectionAsync());
        }

        private async Task DeleteCollectionCommandHandler()
        {
            if (this.dialogService.ShowConfirmation(
                ResourceUtils.GetString("Language_Delete_Collection"),
                ResourceUtils.GetString("Language_Delete_Collection_Confirm").Replace("{collection}", this.SelectedCollection.Collection.Title),
                ResourceUtils.GetString("Language_Yes"),
                ResourceUtils.GetString("Language_No")))
            {
                if (!await this.collectionService.DeleteCollectionAsync(this.selectedCollection))
                {
                    this.dialogService.ShowNotification(
                            ResourceUtils.GetString("Language_Delete_Failed"),
                            ResourceUtils.GetString("Language_Could_Not_Delete_Collection"),
                            ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                }
            }
        }

        private async void GetCollectionsAsync()
        {
            // Remember the selected bank account
            string selectedCollectionId = this.SelectedCollection != null ? this.SelectedCollection.Collection.Id : string.Empty;

            this.Collections = new ObservableCollection<CollectionViewModel>(await this.collectionService.GetCollectionsAsync());

            // Clear active and selected collection. Otherwise they're not updated after editing the selected collection.
            this.ActiveCollection = null;
            this.SelectedCollection = null;
            this.ActiveCollection = this.Collections.Where(c => c.IsActive).FirstOrDefault();
            this.SelectedCollection = this.Collections.Where(c => c.Collection.Id.Equals(selectedCollectionId)).FirstOrDefault();
        }
    }
}
