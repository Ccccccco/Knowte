using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knowte.Services.Contracts.Collection
{
    public class CollectionAddedEventArgs : EventArgs
    {
        public string CollectionId { get; private set; }

        public string CollectionTitle { get; private set; }

        public CollectionAddedEventArgs(string collectionId, string collectionTitle)
        {
            this.CollectionId = collectionId;
            this.CollectionTitle = collectionTitle;
        }
    }
}
