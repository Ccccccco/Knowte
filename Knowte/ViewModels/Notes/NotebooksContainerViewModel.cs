using Digimezzo.Foundation.Core.Utils;
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

        public DelegateCommand AddNotebookCommand { get; set; }

        public DelegateCommand EditNotebookCommand { get; set; }

        public DelegateCommand DeleteNotebookCommand { get; set; }

        public NotebooksContainerViewModel(IUnityContainer container, IDialogService dialogService, ICollectionService collectionService)
        {
            this.container = container;
            this.dialogService = dialogService;
            this.collectionService = collectionService;

            this.AddNotebookCommand = new DelegateCommand(() => this.AddNotebookCommandHandler());
            this.EditNotebookCommand = new DelegateCommand(() => this.EditNotebookCommandHandler());
            this.DeleteNotebookCommand = new DelegateCommand(() => this.DeleteNotebookCommandHandler());
        }

        private void AddNotebookCommandHandler()
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

        private void EditNotebookCommandHandler()
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

        private async void DeleteNotebookCommandHandler()
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
    }
}
