using Knowte.Core.Prism;
using Knowte.Services.Collection;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Knowte.ViewModels.Notes
{
    public class NotesViewModel : BindableBase
    {
        private ICollectionService collectionService;
        private bool hasActiveCollection;
        private bool hasCollections;
        private int allNotesCount;
        private int todayNotesCount;
        private int yesterdayNotesCount;
        private int thisWeekNotesCount;
        private int markedNotesCount;

        private NoteFilter selectedNoteFilter;

        public DelegateCommand ManageCollectionsCommand { get; set; }

        public DelegateCommand LoadedCommand { get; set; }

        public int AllNotesCount
        {
            get { return this.allNotesCount; }
            set { SetProperty<int>(ref this.allNotesCount, value); }
        }

        public int TodayNotesCount
        {
            get { return this.todayNotesCount; }
            set { SetProperty<int>(ref this.todayNotesCount, value); }
        }

        public int YesterdayNotesCount
        {
            get { return this.yesterdayNotesCount; }
            set { SetProperty<int>(ref this.yesterdayNotesCount, value); }
        }

        public NoteFilter SelectedNoteFilter
        {
            get { return (NoteFilter)this.selectedNoteFilter; }
            set
            {
                SetProperty<NoteFilter>(ref this.selectedNoteFilter, value);

                if (this.collectionService != null)
                {
                    this.collectionService.OnNoteFilterChanged(value);
                }
            }
        }

        public int ThisWeekNotesCount
        {
            get { return this.thisWeekNotesCount; }
            set { SetProperty<int>(ref this.thisWeekNotesCount, value); }
        }

        public int MarkedNotesCount
        {
            get { return this.markedNotesCount; }
            set { SetProperty<int>(ref this.markedNotesCount, value); }
        }

        public bool HasActiveCollection
        {
            get { return this.hasActiveCollection; }
            set { SetProperty<bool>(ref this.hasActiveCollection, value); }
        }

        public bool HasCollections
        {
            get { return this.hasCollections; }
            set { SetProperty<bool>(ref this.hasCollections, value); }
        }

        public NotesViewModel(ICollectionService collectionService, IEventAggregator eventAggregator)
        {
            this.collectionService = collectionService;

            this.ManageCollectionsCommand = new DelegateCommand(() => eventAggregator.GetEvent<ManageCollections>().Publish(null));
            this.LoadedCommand = new DelegateCommand(() =>
            {
                this.selectedNoteFilter = this.collectionService.Filter;
                this.GetNotesCountAsync();
            });

            this.EvaluateHasActiveCollectionAsync();
            this.EvaluateHasCollectionsAsync();

            this.collectionService.ActiveCollectionChanged += (_, e) => this.EvaluateHasActiveCollectionAsync();
            this.collectionService.CollectionAdded += (_, e) => this.EvaluateHasCollectionsAsync();
            this.collectionService.CollectionDeleted += (_, e) =>
            {
                this.EvaluateHasActiveCollectionAsync();
                this.EvaluateHasCollectionsAsync();
            };

            this.collectionService.NoteAdded += (_, __) => this.GetNotesCountAsync();
            this.collectionService.NotesDeleted += (_, __) => this.GetNotesCountAsync();
            this.collectionService.NotesMarked += (_, __) => this.GetNotesCountAsync();
            this.collectionService.NotesUnmarked += (_, __) => this.GetNotesCountAsync();
        }

        private async void EvaluateHasActiveCollectionAsync()
        {
            this.HasActiveCollection = await this.collectionService.HasActiveCollection();
        }

        private async void EvaluateHasCollectionsAsync()
        {
            this.HasCollections = (await this.collectionService.GetCollectionsAsync()).Count > 0;
        }

        private async void GetNotesCountAsync()
        {
            NotesCount notesCount = await this.collectionService.GetNotesCountAsync();

            this.AllNotesCount = notesCount.AllNotesCount;
            this.TodayNotesCount = notesCount.TodayNotesCount;
            this.YesterdayNotesCount = notesCount.YesterdayNotesCount;
            this.ThisWeekNotesCount = notesCount.ThisWeekNotesCount;
            this.MarkedNotesCount = notesCount.MarkedNotesCount;
        }
    }
}
