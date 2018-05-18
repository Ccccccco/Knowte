using Digimezzo.Foundation.Core.Settings;
using Digimezzo.Foundation.Core.Utils;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Knowte.ViewModels.Notes
{
    public class NotesContainerViewModel : BindableBase
    {
        private ObservableCollection<NoteViewModel> notes;
        private ICollectionService collectionService;
        private IDialogService dialogService;
        private bool isNotebookSelected;
        private string notebookTitle;
        private int count;

        public IList<NoteViewModel> SelectedNotes { get; set; }

        public DelegateCommand LoadedCommand { get; set; }

        public DelegateCommand MarkNotesCommand { get; set; }

        public DelegateCommand UnmarkNotesCommand { get; set; }

        public DelegateCommand AddNoteCommand { get; set; }

        public DelegateCommand DeleteNotesCommand { get; set; }

        public DelegateCommand<object> SelectedNotesCommand { get; set; }

        public ObservableCollection<NoteViewModel> Notes
        {
            get { return this.notes; }
            set { SetProperty(ref this.notes, value); }
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

        public bool SelectionHasMarkedNotes
        {
            get
            {
                return this.SelectedNotes != null && 
                    this.SelectedNotes.Count > 0 && 
                    this.SelectedNotes.Any(n => n.IsMarked);
            }
        }

        public string NotebookTitle
        {
            get { return this.notebookTitle; }
            set { SetProperty<string>(ref this.notebookTitle, value); }
        }

        public bool CanEdit
        {
            get { return this.SelectedNotes != null && this.SelectedNotes.Count > 0; }
        }

        public NotesContainerViewModel(ICollectionService collectionService, IDialogService dialogService)
        {
            this.collectionService = collectionService;
            this.dialogService = dialogService;

            //this.LoadedCommand = new DelegateCommand(() => );

            this.AddNoteCommand = new DelegateCommand(async () => await this.collectionService.AddNoteAsync(ResourceUtils.GetString("Language_New_Note")));
            this.DeleteNotesCommand = new DelegateCommand(() => this.DeleteNotesAsync());
            this.MarkNotesCommand = new DelegateCommand(() => this.MarkNotesAsync());
            this.UnmarkNotesCommand = new DelegateCommand(() => this.UnmarkNotesAsync());
            this.SelectedNotesCommand = new DelegateCommand<object>((param) => this.SelectedNotesCommandHandler(param));

            this.collectionService.NotebookSelectionChanged += (_, e) =>
            {
                this.NotebookTitle = e.NotebookTitle;
                this.IsNotebookSelected = string.IsNullOrEmpty(e.NotebookTitle) ? false : true;
                this.GetNotesAsync();
            };

            this.collectionService.NoteAdded += (_, __) => this.GetNotesAsync();
            this.collectionService.NotesDeleted += (_, __) => this.GetNotesAsync();
            this.collectionService.NoteFilterChanged += (_, __) => this.GetNotesAsync();
            this.collectionService.NotesMarked += (_, e) => this.UpdateNotesMarkAsync(e.NoteIds, true);
            this.collectionService.NotesUnmarked += (_, e) => this.UpdateNotesMarkAsync(e.NoteIds, false);

            this.IsNotebookSelected = string.IsNullOrEmpty(this.NotebookTitle) ? false : true;
        }

        private void SelectedNotesCommandHandler(object param)
        {
            if (param != null)
            {
                this.SelectedNotes = new List<NoteViewModel>();

                foreach (NoteViewModel item in (IList)param)
                {
                    this.SelectedNotes.Add(item);
                }
            }

            this.RaisePropertyChanged(nameof(this.CanEdit));
            this.RaisePropertyChanged(nameof(this.SelectionHasMarkedNotes));
        }

        private async void DeleteNotesAsync()
        {
            string deleteMessage = ResourceUtils.GetString("Language_Delete_Notes_Confirm");

            if (this.SelectedNotes.Count == 1)
            {
                deleteMessage = ResourceUtils.GetString("Language_Delete_Note_Confirm").Replace("{note}", this.SelectedNotes[0].Title);
            }

            if (this.dialogService.ShowConfirmation(
                 ResourceUtils.GetString("Language_Delete_Note"),
                 deleteMessage,
                 ResourceUtils.GetString("Language_Yes"),
                 ResourceUtils.GetString("Language_No")))
            {
                if (!await this.collectionService.DeleteNotesAsync(this.SelectedNotes))
                {
                    this.dialogService.ShowNotification(
                            ResourceUtils.GetString("Language_Delete_Failed"),
                            ResourceUtils.GetString("Language_Could_Not_Delete_Note"),
                            ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
                }
            }
        }

        private async void MarkNotesAsync()
        {
            if (!await this.collectionService.SetNotesMarkAsync(this.SelectedNotes, true))
            {
                this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Mark_Note_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Mark_Note"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
            }
        }

        private async void UnmarkNotesAsync()
        {
            if (!await this.collectionService.SetNotesMarkAsync(this.SelectedNotes, false))
            {
                this.dialogService.ShowNotification(
                        ResourceUtils.GetString("Language_Mark_Note_Failed"),
                        ResourceUtils.GetString("Language_Could_Not_Mark_Note"),
                        ResourceUtils.GetString("Language_Ok"), true, ResourceUtils.GetString("Language_Log_File"));
            }
        }

        private async void UpdateNotesMarkAsync(IList<string> noteIds, bool isMarked)
        {
            IList<NoteViewModel> notesToChange = this.Notes.Where(n => noteIds.Contains(n.Id)).ToList();

            // If we're unmarking notes and the marked notes page is selected, we need to remove the notes from the list.
            if (!isMarked && this.collectionService.Filter.Equals(NoteFilter.Marked))
            {
                await Task.Run(() =>
                {
                    foreach (NoteViewModel noteToChange in notesToChange)
                    {
                        Application.Current.Dispatcher.Invoke(() => this.notes.Remove(noteToChange));
                    }
                });

                return;
            }

            // Update selected notes
            if (this.SelectedNotes != null && this.SelectedNotes.Count > 0)
            {
                await Task.Run(() =>
                {
                    foreach (NoteViewModel noteToChange in notesToChange)
                    {

                        NoteViewModel note = this.SelectedNotes.Where(n => n.Id.Equals(noteToChange.Id)).FirstOrDefault();

                        if (note != null)
                        {
                            Application.Current.Dispatcher.Invoke(() => note.IsMarked = isMarked);
                        }
                    }
                });
            }

            // Update all notes
            if (this.notes != null && this.notes.Count > 0)
            {
                await Task.Run(() =>
                {
                    foreach (NoteViewModel noteToChange in notesToChange)
                    {
                        NoteViewModel note = this.notes.Where(n => n.Id.Equals(noteToChange.Id)).FirstOrDefault();

                        if (note != null)
                        {
                            Application.Current.Dispatcher.Invoke(() => note.IsMarked = isMarked);
                        }
                    }
                });
            }

            this.RaisePropertyChanged(nameof(this.SelectionHasMarkedNotes));
        }

        private async void GetNotesAsync()
        {
            this.Notes = new ObservableCollection<NoteViewModel>(await this.collectionService.GetNotesAsync(SettingsClient.Get<bool>("Appearance", "SortByModificationDate")));

            // Set the count
            this.Count = this.Notes != null ? this.Notes.Count : 0;
        }
    }
}
