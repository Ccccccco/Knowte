using Digimezzo.Foundation.Core.Utils;
using Knowte.Presentation.Contracts.Entities;
using Knowte.Services.Constracts.Dialog;
using Knowte.Services.Contracts.Collection;
using Knowte.ViewModels.Dialogs;
using Knowte.Views.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Knowte.ViewModels.Notes
{
    public class NotebooksContainerViewModel : BindableBase
    {
        private IUnityContainer container;
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
            set { SetProperty<ObservableCollection<NotebookViewModel>>(ref this.notebooks, value); }
        }

        public NotebookViewModel SelectedNotebook
        {
            get { return this.selectedNotebook; }
            set
            {
                SetProperty<NotebookViewModel>(ref this.selectedNotebook, value);
            }
        }

        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand AddNotebookCommand { get; set; }

        public DelegateCommand EditNotebookCommand { get; set; }

        public DelegateCommand DeleteNotebookCommand { get; set; }

        public NotebooksContainerViewModel(IUnityContainer container, IDialogService dialogService, ICollectionService collectionService)
        {
            this.container = container;
            this.dialogService = dialogService;
            this.collectionService = collectionService;

            this.LoadedCommand = new DelegateCommand(async() => await this.GetNotebooksAsync());

            this.AddNotebookCommand = new DelegateCommand(() => this.AddNotebook());
            this.EditNotebookCommand = new DelegateCommand(() => this.EditNotebook());
            this.DeleteNotebookCommand = new DelegateCommand(async() => await this.DeleteNotebookAsync());
        }

        private void AddNotebook()
        {
            AddNotebook view = this.container.Resolve<AddNotebook>();
            AddNotebookViewModel viewModel = this.container.Resolve<AddNotebookViewModel>();
            view.DataContext = viewModel;

            this.dialogService.ShowCustom(
                ResourceUtils.GetString("Language_Add_Notebook"),
                view,
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
            EditNotebook view = this.container.Resolve<EditNotebook>();
            EditNotebookViewModel viewModel = this.container.Resolve<EditNotebookViewModel>(new DependencyOverride(typeof(NotebookViewModel), this.SelectedNotebook));
            view.DataContext = viewModel;

            this.dialogService.ShowCustom(
                ResourceUtils.GetString("Language_Edit_Notebook"),
                view,
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

        private async Task DeleteNotebookAsync()
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

        private async Task GetNotebooksAsync()
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
