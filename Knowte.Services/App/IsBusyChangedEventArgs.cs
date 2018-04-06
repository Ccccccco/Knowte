using System;

namespace Knowte.Services.App
{
    public class IsBusyChangedEventArgs : EventArgs
    {
        public bool IsBusy { get; set; }
    }
}
