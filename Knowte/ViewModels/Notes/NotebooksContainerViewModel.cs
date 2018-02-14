using Prism.Commands;
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

        public DelegateCommand AddNotebookCommand { get; set; }

        public DelegateCommand EditNotebookCommand { get; set; }

        public DelegateCommand DeleteNotebookCommand { get; set; }

        public NotebooksContainerViewModel()
        {
                    
        }
    }
}
