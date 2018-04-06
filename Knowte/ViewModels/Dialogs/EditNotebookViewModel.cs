using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.Entities;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace Knowte.ViewModels.Dialogs
{
    public class EditNotebookViewModel : BindableBase
    {
        NotebookViewModel notebook;
        private string title;
        IDialogService dialogService;
        ICollectionService collectionService;

        public string Title
        {
            get { return this.title; }
            set { SetProperty<string>(ref this.title, value); }
        }

        public EditNotebookViewModel(NotebookViewModel notebook, IDialogService dialogService, ICollectionService collectionService)
        {
            this.notebook = notebook;
            this.dialogService = dialogService;
            this.collectionService = collectionService;

            if (notebook != null)
            {
                this.Title = notebook.Title;
            }
        }

        public async Task<bool> EditNotebookAsync()
        {
            ChangeNotebookResult result = ChangeNotebookResult.Ok;

            result = await this.collectionService.EditNotebookAsync(this.notebook, this.title);

            switch (result)
            {
                case ChangeNotebookResult.Invalid:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Edit_Failed"),
                        ResourceUtils.GetString("Language_Please_Provide_Title_For_Notebook"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    return false;
                case ChangeNotebookResult.Duplicate:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Edit_Failed"),
                        ResourceUtils.GetString("Language_Notebook_With_Title_Already_Exists"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    return false;
                case ChangeNotebookResult.Error:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Edit_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Edit_Notebook_Check_Log_File"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                    return false;
                case ChangeNotebookResult.Ok:
                default:
                    break;
            }

            return result.Equals(ChangeNotebookResult.Ok);
        }
    }
}
