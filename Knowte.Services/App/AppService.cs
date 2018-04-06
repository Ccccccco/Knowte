using System.Threading.Tasks;

namespace Knowte.Services.App
{
    public class AppService : IAppService
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                this.OnIsBusyChanged(value);
            }
        }

        public event IsBusyChangedEventHandler IsBusyChanged = delegate { };

        private void OnIsBusyChanged(bool isBusy)
        {
            this.isBusy = isBusy;
            Task.Delay(500).ContinueWith(t => this.IsBusyChanged(this, new IsBusyChangedEventArgs { IsBusy = this.isBusy }));
        }
    }
}
