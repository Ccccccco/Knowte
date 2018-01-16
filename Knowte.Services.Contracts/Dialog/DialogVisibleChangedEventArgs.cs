using System;

namespace Knowte.Services.Constracts.Dialog
{
    public class DialogVisibleChangedEventArgs : EventArgs
    {
        public bool HasOpenDialogs { get; set; }
    }
}