using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.ViewModels.Dialogs
{
    public class AddCollectionViewModel : BindableBase
    {
        private string title;
        private bool activate;
        private IDialogService dialogService;
        private ICollectionService collectionService;

        public string Title
        {
            get { return this.title; }
            set { SetProperty<string>(ref this.title, value); }
        }

        public bool Activate
        {
            get { return this.activate; }
            set { SetProperty<bool>(ref this.activate, value); }
        }

        public AddCollectionViewModel(IDialogService dialogService, ICollectionService collectionService)
        {
            this.dialogService = dialogService;
            this.collectionService = collectionService;

            this.SetActivateAsync();
        }

        private async void SetActivateAsync()
        {
            List<CollectionViewModel> collections = await this.collectionService.GetCollectionsAsync();

            if(collections == null || collections.Count == 0)
            {
                this.Activate = true;
                return;
            }

            this.Activate = false;
        }

        public async Task<bool> AddCollectionAsync()
        {
            ChangeCollectionResult result = ChangeCollectionResult.Ok;

            result = await this.collectionService.AddCollectionAsync(this.title, this.activate);

            switch (result)
            {
                case ChangeCollectionResult.Invalid:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Add_Failed"),
                        ResourceUtils.GetString("Language_Please_Provide_Title_For_Collection"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    break;
                case ChangeCollectionResult.Duplicate:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Add_Failed"),
                        ResourceUtils.GetString("Language_Collection_With_Title_Already_Exists"),
                        ResourceUtils.GetString("Language_Ok"), false, string.Empty);
                    break;
                case ChangeCollectionResult.Error:
                    this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Add_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Add_Collection_Check_Log_File"),
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
