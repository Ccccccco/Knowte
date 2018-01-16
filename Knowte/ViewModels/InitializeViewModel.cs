using Digimezzo.Utilities.Settings;
using Knowte.Core.Base;
using Prism.Mvvm;

namespace Knowte.ViewModels
{
    public class InitializeViewModel : BindableBase
    {
        public string InitializeText
        {
            get
            {
                if (SettingsClient.Get<bool>("Configuration", "ShowWelcome"))
                {
                    return $"Preparing {ProductInformation.ApplicationName}";
                }

                return $"Updating {ProductInformation.ApplicationName}";
            }
        }
    }
}
