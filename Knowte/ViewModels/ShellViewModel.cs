using Knowte.Services.Constracts.Dialog;
using Knowte.Services.Contracts.App;
using Prism.Mvvm;

namespace Knowte.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private bool isOverlayVisible;
        private bool isBusyIndicatorVisible;

        public bool IsBusyIndicatorVisible
        {
            get { return this.isBusyIndicatorVisible; }
            set { SetProperty(ref this.isBusyIndicatorVisible, value); }
        }

        public bool IsOverlayVisible
        {
            get { return this.isOverlayVisible; }
            set { SetProperty(ref this.isOverlayVisible, value); }
        }

        public ShellViewModel(IDialogService dialogService, IAppService appService)
        {
            dialogService.DialogVisibleChanged += (_, e) => this.IsOverlayVisible = e.HasOpenDialogs;
            appService.IsBusyChanged += (_, e) => this.IsBusyIndicatorVisible = e.IsBusy;
        }
    }
}
