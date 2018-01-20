using System;

namespace Knowte.Services.Contracts.Collection
{
    public class CollectionChangedEventArgs : EventArgs
    {
        public string CollectionId { get; private set; }

        public CollectionChangedEventArgs(string collectionId)
        {
            this.CollectionId = collectionId;
        }
    }
}
