using System;
using System.Collections.Generic;

namespace Knowte.Services.Collection
{
    public class NotesChangedEventArgs : EventArgs
    {
        public IList<string> NoteIds { get; private set; }

        public NotesChangedEventArgs(IList<string> noteIds)
        {
            this.NoteIds = noteIds;
        }
    }
}