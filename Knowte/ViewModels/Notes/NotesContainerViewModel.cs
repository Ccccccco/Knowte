using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Knowte.ViewModels.Notes
{
    public class NotesContainerViewModel : BindableBase
    {
        private ObservableCollection<NoteViewModel> notes;
        private NoteViewModel selectedNote;
        private ICollectionService collectionService;
        private bool isNotebookSelected;
        private string notebookTitle;
        private int count;
        
        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand MarkNoteCommand { get; set; }

        public DelegateCommand AddNoteCommand { get; set; }

        public DelegateCommand DeleteNoteCommand { get; set; }

        public ObservableCollection<NoteViewModel> Notes
        {
            get { return this.notes; }
            set { SetProperty(ref this.notes, value); }
        }

        public NoteViewModel SelectedNote
        {
            get { return this.selectedNote; }
            set
            {
                SetProperty(ref this.selectedNote, value);
                this.RaisePropertyChanged(nameof(this.CanEdit));
            }
        }

        public int Count
        {
            get { return this.count; }
            set { SetProperty<int>(ref this.count, value); }
        }

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

        public bool CanEdit
        {
            get { return this.selectedNote != null; }
        }

        public NotesContainerViewModel(ICollectionService collectionService)
        {
            //this.LoadedCommand = new DelegateCommand(() => );

            this.AddNoteCommand = new DelegateCommand(async() => await this.collectionService.CreateNewNoteAsync(ResourceUtils.GetString("Language_New_Note")));

            this.collectionService = collectionService;

            this.collectionService.NotebookSelectionChanged += (_, e) =>
            {
                this.NotebookTitle = e.NotebookTitle;
                this.IsNotebookSelected = string.IsNullOrEmpty(e.NotebookTitle) ? false : true;
                this.GetNotesAsync();
            };

            this.IsNotebookSelected = string.IsNullOrEmpty(this.NotebookTitle) ? false : true;
        }

        private async void GetNotesAsync()
        {
            // this.Notes = new ObservableCollection<NoteViewModel>(await this.collectionService.GetNotesAsync());

            // Set the count
            this.Count = this.Notes != null ? this.Notes.Count : 0;
        }
    }
}
