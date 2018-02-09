using Prism.Mvvm;

namespace Knowte.ViewModels.Notes
{
    public class NotebooksContainerViewModel : BindableBase
    {
        private int count;

        public int Count
        {
            get { return count; }
            set { SetProperty<int>(ref this.count, value); }
        }

    }
}
