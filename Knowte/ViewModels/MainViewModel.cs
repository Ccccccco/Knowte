using Prism.Commands;
using Prism.Mvvm;

namespace Knowte.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private bool showCollections;

        public bool ShowCollections
        {
            get { return this.showCollections; }
            set
            {
                SetProperty<bool>(ref this.showCollections, value);
                RaisePropertyChanged(nameof(ContentIndex));
            }
        }

        public int ContentIndex
        {
            get { return this.showCollections ? 0 : 1; }
        }

        public DelegateCommand PaneClosedCommand { get; set; }

        public MainViewModel()
        {
            this.PaneClosedCommand = new DelegateCommand(() => this.ShowCollections = false);
        }
    }
}
