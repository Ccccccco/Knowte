using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Knowte.Services.Dialog
{
    public delegate void DialogVisibleChangedEventHandler(object sender, DialogVisibleChangedEventArgs e);

    public interface IDialogService
    {
        bool ShowConfirmation(string title, string content, string okText, string cancelText);
        bool ShowNotification(string title, string content, string okText, bool showViewLogs, string viewLogsText = "Log file");
        bool ShowCustom(string title, object datacontext, int width, int height, bool canResize, bool autoSize, bool showTitle, bool showCancelButton, string okText, string cancelText, Func<Task<bool>> callback);
        event DialogVisibleChangedEventHandler DialogVisibleChanged;
    }
}
