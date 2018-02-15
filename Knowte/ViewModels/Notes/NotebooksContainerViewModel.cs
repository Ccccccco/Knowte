using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Constracts.Dialog;
using Knowte.ViewModels.Dialogs;
using Knowte.Views.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;

namespace Knowte.ViewModels.Notes
{
    public class NotebooksContainerViewModel : BindableBase
    {
        private IUnityContainer container;
        private IDialogService dialogService;
        private int count;

        public int Count
        {
            get { return count; }
            set { SetProperty<int>(ref this.count, value); }
        }

        public DelegateCommand AddNotebookCommand { get; set; }

        public DelegateCommand EditNotebookCommand { get; set; }

        public DelegateCommand DeleteNotebookCommand { get; set; }

        public NotebooksContainerViewModel(IUnityContainer container, IDialogService dialogService)
        {
            this.container = container;
            this.dialogService = dialogService;

            this.AddNotebookCommand = new DelegateCommand(() => this.AddNotebookCommandHandler());
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
    }
}
