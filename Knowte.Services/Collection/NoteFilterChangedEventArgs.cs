using System;

namespace Knowte.Services.Collection
{
    public class NoteFilterChangedEventArgs : EventArgs
    {
        public NoteFilter Filter { get; set; }

        public NoteFilterChangedEventArgs(NoteFilter filter)
        {
            this.Filter = filter;
        }
    }
}
