using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.Entities;
using Knowte.ViewModels.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Knowte.ViewModels.Notes
{
    public class NotebooksContainerViewModel : BindableBase
    {
        private IDialogService dialogService;
        private ICollectionService collectionService;
        private int count;
        private ObservableCollection<NotebookViewModel> notebooks;
        private NotebookViewModel selectedNotebook;

        public int Count
        {
            get { return count; }
            set { SetProperty<int>(ref this.count, value); }
        }

        public ObservableCollection<NotebookViewModel> Notebooks
        {
            get { return this.notebooks; }
            set { SetProperty(ref this.notebooks, value); }
        }

        public NotebookViewModel SelectedNotebook
        {
            get { return this.selectedNotebook; }
            set
            {
                SetProperty(ref this.selectedNotebook, value);
                this.RaisePropertyChanged(nameof(this.CanEdit));
            }
        }

        public bool CanEdit
        {
            get { return this.selectedNotebook != null && !this.selectedNotebook.IsDefault; }
        }

        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand AddNotebookCommand { get; set; }

        public DelegateCommand EditNotebookCommand { get; set; }

        public DelegateCommand DeleteNotebookCommand { get; set; }

        public NotebooksContainerViewModel(IDialogService dialogService, ICollectionService collectionService)
        {
            this.dialogService = dialogService;
            this.collectionService = collectionService;

            this.LoadedCommand = new DelegateCommand(() => this.GetNotebooksAsync());

            this.AddNotebookCommand = new DelegateCommand(() => this.AddNotebook());
            this.EditNotebookCommand = new DelegateCommand(() => this.EditNotebook());
            this.DeleteNotebookCommand = new DelegateCommand(() => this.DeleteNotebookAsync());

            this.collectionService.NotebookAdded += (_, __) => this.GetNotebooksAsync();
            this.collectionService.NotebookEdited += (_, __) => this.GetNotebooksAsync();
            this.collectionService.NotebookDeleted += (_, __) => this.GetNotebooksAsync();
        }

        private void AddNotebook()
        {
            var viewModel = new AddNotebookViewModel(this.dialogService, this.collectionService);

            this.dialogService.ShowCustom(
                ResourceUtils.GetString("Language_Add_Notebook"),
                viewModel,
                420,
                0,
                false,
                true,
                true,
                true,
                ResourceUtils.GetString("Language_Ok"),
                ResourceUtils.GetString("Language_Cancel"),
                async () => await viewModel.AddNotebookAsync());
        }

        private void EditNotebook()
        {
            var viewModel = new EditNotebookViewModel(this.SelectedNotebook, this.dialogService, this.collectionService);
            
            this.dialogService.ShowCustom(
                ResourceUtils.GetString("Language_Edit_Notebook"),
                viewModel,
                420,
                0,
                false,
                true,
                true,
                true,
                ResourceUtils.GetString("Language_Ok"),
                ResourceUtils.GetString("Language_Cancel"),
                async () => await viewModel.EditNotebookAsync());
        }

        private async void DeleteNotebookAsync()
        {
            if (this.dialogService.ShowConfirmation(
                 ResourceUtils.GetString("Language_Delete_Notebook"),
                 ResourceUtils.GetString("Language_Delete_Notebook_Confirm").Replace("{notebook}", this.selectedNotebook.Title),
                 ResourceUtils.GetString("Language_Yes"),
                 ResourceUtils.GetString("Language_No")))
            {
                if (!await this.collectionService.DeleteNotebookAsync(this.selectedNotebook))
                {
                    this.dialogService.ShowNotification(
                            ResourceUtils.GetString("Language_Delete_Failed"),
                            ResourceUtils.GetString("Language_Could_Not_Delete_Notebook"),
                            ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                }
            }
        }

        private async void GetNotebooksAsync()
        {
            // Remember the selected notebook
            string selectedNotebookId = this.selectedNotebook != null ? this.selectedNotebook.Id : string.Empty;

            this.Notebooks = new ObservableCollection<NotebookViewModel>(await this.collectionService.GetNotebooksAsync());

            // Clear selected notebook. Otherwise it's not updated after editing the selected notebook.
            this.SelectedNotebook = null;
            this.SelectedNotebook = this.Notebooks.Where(n => n.Id.Equals(selectedNotebookId)).FirstOrDefault();
        }
    }
}
