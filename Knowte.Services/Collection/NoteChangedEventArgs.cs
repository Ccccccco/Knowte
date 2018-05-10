using System;

namespace Knowte.Services.Collection
{
    public class NoteChangedEventArgs : EventArgs
    {
        public string NoteId { get; private set; }

        public NoteChangedEventArgs(string noteId)
        {
            this.NoteId = noteId;
        }
    }
}