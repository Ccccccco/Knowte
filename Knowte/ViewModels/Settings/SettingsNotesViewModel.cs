using Digimezzo.Foundation.Core.Settings;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace Knowte.ViewModels.Settings
{
    public class SettingsNotesViewModel : BindableBase
    {
        private bool showExactDates;

        public bool ShowExactDates
        {
            get { return this.showExactDates; }

            set
            {
                SettingsClient.Set<bool>("Notes", "ShowExactDates", value);
                SetProperty<bool>(ref this.showExactDates, value);
            }
        }

        public SettingsNotesViewModel()
        {
            this.GetTogglesAsync();
        }

        private async void GetTogglesAsync()
        {
            await Task.Run(() =>
            {
                this.showExactDates = SettingsClient.Get<bool>("Notes", "ShowExactDates");
            });
        }
    }
}
