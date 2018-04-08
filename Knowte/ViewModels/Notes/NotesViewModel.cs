using Knowte.Services.Collection;
using Prism.Mvvm;

namespace Knowte.ViewModels.Notes
{
    public class NotesViewModel : BindableBase
    {
        private ICollectionService collectionService;
        private bool hasActiveCollection;

        public bool HasActiveCollection
        {
            get { return this.hasActiveCollection; }
            set { SetProperty<bool>(ref this.hasActiveCollection, value); }
        }

        public NotesViewModel(ICollectionService collectionService)
        {
            this.collectionService = collectionService;

            this.EvaluateHasActiveCollectionAsync();

            this.collectionService.ActiveCollectionChanged += (_, e) => this.EvaluateHasActiveCollectionAsync();
            this.collectionService.CollectionDeleted += (_, e) => this.EvaluateHasActiveCollectionAsync();
        }

        private async void EvaluateHasActiveCollectionAsync()
        {
            this.HasActiveCollection = await this.collectionService.HasActiveCollection();
        }
    }
}
