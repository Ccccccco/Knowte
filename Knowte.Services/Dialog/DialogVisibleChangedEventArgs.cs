using System;

namespace Knowte.Services.Dialog
{
    public class DialogVisibleChangedEventArgs : EventArgs
    {
        public bool HasOpenDialogs { get; set; }
    }
}