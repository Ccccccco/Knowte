using System;

namespace Knowte.Services.Contracts.Collection
{
    public class NotebookChangedEventArgs : EventArgs
    {
        public string NotebookId { get; private set; }

        public NotebookChangedEventArgs(string notebookId)
        {
            this.NotebookId = notebookId;
        }
    }
}
