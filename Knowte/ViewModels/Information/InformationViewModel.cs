using Digimezzo.Foundation.Core.IO;
using Digimezzo.Foundation.Core.Packaging;
using Digimezzo.Foundation.Core.Utils;
using Knowte.Core.Base;
using Knowte.Services.Constracts.Dialog;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;

namespace Knowte.ViewModels.Information
{
    public class InformationViewModel : BindableBase
    {
        private IUnityContainer container;
        private IDialogService dialogService;
        private Package package;

        public Package Package
        {
            get { return this.package; }
            set { SetProperty<Package>(ref this.package, value); }
        }

        public DelegateCommand<string> OpenLinkCommand { get; set; }

        public DelegateCommand ShowLicenseCommand { get; set; }

        public InformationViewModel(IUnityContainer container, IDialogService dialogService)
        {
            this.container = container;
            this.dialogService = dialogService;

            this.Package = new Package(ProcessExecutable.Name(), ProcessExecutable.AssemblyVersion());

            this.ShowLicenseCommand = new DelegateCommand(() =>
            {
                var view = this.container.Resolve<Views.Information.License>();

                this.dialogService.ShowCustom(
                    ResourceUtils.GetString("Language_License"),
                    view,
                    400,
                    0,
                    false,
                    true,
                    true,
                    false,
                    ResourceUtils.GetString("Language_Ok"),
                    string.Empty,
                    null);
            });

            this.OpenLinkCommand = new DelegateCommand<string>((string link) => SafeActions.TryOpenLink(link));
        }
    }
}
