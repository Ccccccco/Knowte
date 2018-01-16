using Knowte.Services.Constracts.Dialog;
using Prism.Mvvm;

namespace Knowte.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private bool isOverlayVisible;

        public bool IsOverlayVisible
        {
            get { return this.isOverlayVisible; }
            set { SetProperty<bool>(ref this.isOverlayVisible, value); }
        }

        public ShellViewModel(IDialogService dialogService)
        {
            dialogService.DialogVisibleChanged += (_, e) => this.IsOverlayVisible = e.HasOpenDialogs;
        }
    }
}
