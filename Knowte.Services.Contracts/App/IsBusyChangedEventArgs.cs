using System;

namespace Knowte.Services.Contracts.App
{
    public class IsBusyChangedEventArgs : EventArgs
    {
        public bool IsBusy { get; set; }
    }
}
