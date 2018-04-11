using System;

namespace Knowte.Services.Collection
{
    public class NotebookSelectionChangedEventArgs : EventArgs
    {
        public string NotebookId { get; private set; }
        public string NotebookTitle { get; private set; }

        public NotebookSelectionChangedEventArgs(string notebookId, string notebookTitle)
        {
            this.NotebookId = notebookId;
            this.NotebookTitle = notebookTitle;
        }
    }
}
