using Digimezzo.Foundation.Core.Settings;
using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Knowte.ViewModels.Notes
{
    public class NotesContainerViewModel : BindableBase
    {
        private ObservableCollection<NoteViewModel> notes;
        private NoteViewModel selectedNote;
        private ICollectionService collectionService;
        private IDialogService dialogService;
        private bool isNotebookSelected;
        private string notebookTitle;
        private int count;

        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand MarkNoteCommand { get; set; }

        public DelegateCommand UnmarkNoteCommand { get; set; }

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

        //public bool IsSelectedNoteMarked
        //{
        //    get
        //    {
        //        if (this.selectedNote == null)
        //        {
        //            return false;
        //        }

        //        return this.selectedNote.IsMarked;
        //    }
        //}

        public NotesContainerViewModel(ICollectionService collectionService, IDialogService dialogService)
        {
            this.collectionService = collectionService;
            this.dialogService = dialogService;

            //this.LoadedCommand = new DelegateCommand(() => );

            this.AddNoteCommand = new DelegateCommand(async () => await this.collectionService.AddNoteAsync(ResourceUtils.GetString("Language_New_Note")));
            this.DeleteNoteCommand = new DelegateCommand(() => this.DeleteNoteAsync());
            this.MarkNoteCommand = new DelegateCommand(() => this.MarkNoteAsync());
            this.UnmarkNoteCommand = new DelegateCommand(() => this.UnmarkNoteAsync());

            this.collectionService.NotebookSelectionChanged += (_, e) =>
            {
                this.NotebookTitle = e.NotebookTitle;
                this.IsNotebookSelected = string.IsNullOrEmpty(e.NotebookTitle) ? false : true;
                this.GetNotesAsync();
            };

            this.collectionService.NoteAdded += (_, __) => this.GetNotesAsync();
            this.collectionService.NoteDeleted += (_, __) => this.GetNotesAsync();
            this.collectionService.NoteMarked += (_, e) => this.UpdateNoteMarkAsync(e.NoteId, true);
            this.collectionService.NoteUnmarked += (_, e) => this.UpdateNoteMarkAsync(e.NoteId, false);

            this.IsNotebookSelected = string.IsNullOrEmpty(this.NotebookTitle) ? false : true;
        }

        private async void DeleteNoteAsync()
        {
            if (this.dialogService.ShowConfirmation(
                 ResourceUtils.GetString("Language_Delete_Note"),
                 ResourceUtils.GetString("Language_Delete_Note_Confirm").Replace("{note}", this.selectedNote.Title),
                 ResourceUtils.GetString("Language_Yes"),
                 ResourceUtils.GetString("Language_No")))
            {
                if (!await this.collectionService.DeleteNoteAsync(this.selectedNote))
                {
                    this.dialogService.ShowNotification(
                            ResourceUtils.GetString("Language_Delete_Failed"),
                            ResourceUtils.GetString("Language_Could_Not_Delete_Note"),
                            ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                }
            }
        }

        private async void MarkNoteAsync()
        {
            if (!await this.collectionService.SetNoteMarkAsync(this.selectedNote, true))
            {
                this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Mark_Note_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Mark_Note"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
            }
        }

        private async void UnmarkNoteAsync()
        {
            if (!await this.collectionService.SetNoteMarkAsync(this.selectedNote, false))
            {
                this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Mark_Note_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Mark_Note"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
            }
        }

        private async void UpdateNoteMarkAsync(string noteId, bool isMarked)
        {
            // Update selected note
            if (selectedNote != null && selectedNote.Id.Equals(noteId))
            {
                this.selectedNote.IsMarked = isMarked;
            }

            // Update note in the list
            if(this.notes == null || this.notes.Count == 0)
            {
                return;
            }

            await Task.Run(() =>
            {
                foreach (NoteViewModel note in this.notes)
                {
                    if (note.Id.Equals(noteId))
                    {
                        Application.Current.Dispatcher.Invoke(() => note.IsMarked = isMarked);
                    }
                }
            });
        }

        private async void GetNotesAsync()
        {
            this.Notes = new ObservableCollection<NoteViewModel>(await this.collectionService.GetNotesAsync(SettingsClient.Get<bool>("Appearance", "SortByModificationDate")));

            // Set the count
            this.Count = this.Notes != null ? this.Notes.Count : 0;
        }
    }
}
