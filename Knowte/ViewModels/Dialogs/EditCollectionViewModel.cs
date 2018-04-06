using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.Entities;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace Knowte.ViewModels.Dialogs
{
    public class EditCollectionViewModel : BindableBase
    {
        private CollectionViewModel collection;
        private string title;
        private IDialogService dialogService;
        private ICollectionService collectionService;

        public string Title
        {
            get { return this.title; }
            set { SetProperty<string>(ref this.title, value); }
        }

        public EditCollectionViewModel(CollectionViewModel collection, IDialogService dialogService, ICollectionService collectionService)
        {
            this.collection = collection;
            this.dialogService = dialogService;
            this.collectionService = collectionService;

            if (collection != null)
            {
                this.Title = collection.Title;
            }
        }

        public async Task<bool> EditCollectionAsync()
        {
            ChangeCollectionResult result = ChangeCollectionResult.Ok;

            result = await this.collectionService.EditCollectionAsync(this.collection, this.title);

            switch (result)
            {
                case ChangeCollectionResult.Invalid:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Edit_Failed"),
                        ResourceUtils.GetString("Language_Please_Provide_Title_For_Collection"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    break;
                case ChangeCollectionResult.Duplicate:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Edit_Failed"),
                        ResourceUtils.GetString("Language_Collection_With_Title_Already_Exists"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    break;
                case ChangeCollectionResult.Error:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Edit_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Edit_Collection"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                    break;
                case ChangeCollectionResult.Ok:
                default:
                    break;
            }

            return result.Equals(ChangeCollectionResult.Ok);
        }
    }
}
