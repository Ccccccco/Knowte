using Knowte.Services.Collection;
using Prism.Commands;
using Prism.Mvvm;

namespace Knowte.ViewModels.Notes
{
    public class NotesContainerViewModel : BindableBase
    {
        private ICollectionService collectionService;
        private bool isNotebookSelected;
        private string notebookTitle;

        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand MarkNoteCommand { get; set; }

        public DelegateCommand AddNoteCommand { get; set; }

        public DelegateCommand DeleteNoteCommand { get; set; }

        public bool IsNotebookSelected
        {
            get { return this.isNotebookSelected; }
            set { SetProperty<bool>(ref this.isNotebookSelected, value); }
        }

        public string NotebookTitle
        {
            get { return this.notebookTitle; }
            set { SetProperty<string>(ref this.notebookTitle, value); }
        }

        public NotesContainerViewModel(ICollectionService collectionService)
        {
            //this.LoadedCommand = new DelegateCommand(() => );

            this.collectionService = collectionService;

            this.collectionService.NotebookSelectionChanged += (_, e) =>
            {
                this.NotebookTitle = e.NotebookTitle;
                this.IsNotebookSelected = string.IsNullOrEmpty(e.NotebookTitle) ? false : true;
            };

            this.IsNotebookSelected = string.IsNullOrEmpty(this.NotebookTitle) ? false : true;
        }
    }
}
