using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace Knowte.ViewModels.Dialogs
{
    public class AddNotebookViewModel : BindableBase
    {
        private string title;
        private IDialogService dialogService;
        private ICollectionService collectionService;

        public string Title
        {
            get { return this.title; }
            set { SetProperty<string>(ref this.title, value); }
        }

        public AddNotebookViewModel(IDialogService dialogService, ICollectionService collectionService)
        {
            this.dialogService = dialogService;
            this.collectionService = collectionService;
        }

        public async Task<bool> AddNotebookAsync()
        {
            ChangeNotebookResult result = ChangeNotebookResult.Ok;

            result = await this.collectionService.AddNotebookAsync(this.title);

            switch (result)
            {
                case ChangeNotebookResult.Invalid:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Add_Failed"),
                        ResourceUtils.GetString("Language_Please_Provide_Title_For_Notebook"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    break;
                case ChangeNotebookResult.Duplicate:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Add_Failed"),
                        ResourceUtils.GetString("Language_Notebook_With_Title_Already_Exists"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    break;
                case ChangeNotebookResult.Error:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Add_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Add_Notebook_Check_Log_File"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                    break;
                case ChangeNotebookResult.Ok:
                default:
                    break;
            }

            return result.Equals(ChangeNotebookResult.Ok);
        }
    }
}
