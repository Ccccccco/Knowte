using Digimezzo.Foundation.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Knowte.Services.Dialog
{
    public class DialogService : IDialogService
    {
        private List<Windows10BorderlessWindow> openDialogs;

        public event DialogVisibleChangedEventHandler DialogVisibleChanged = delegate { };

        public DialogService()
        {
            this.openDialogs = new List<Windows10BorderlessWindow>();
        }

        private void ShowDialog(Windows10BorderlessWindow win)
        {
            foreach (Windows10BorderlessWindow dlg in this.openDialogs)
            {
                dlg.IsOverlayVisible = true;
            }

            this.openDialogs.Add(win);
            this.DialogVisibleChanged(this, new DialogVisibleChangedEventArgs { HasOpenDialogs = this.openDialogs.Count > 0 });

            win.ShowDialog();
            this.openDialogs.Remove(win);
            this.DialogVisibleChanged(this, new DialogVisibleChangedEventArgs { HasOpenDialogs = this.openDialogs.Count > 0 });

            foreach (Windows10BorderlessWindow dlg in this.openDialogs)
            {
                dlg.IsOverlayVisible = false;
            }
        }

        public bool ShowNotification(string title, string content, string okText, bool showViewLogs, string viewLogsText = "Log file")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NotificationDialog dialog = new NotificationDialog(title: title, content: content, okText: okText, showViewLogs: showViewLogs, viewLogsText: viewLogsText);
                this.ShowDialog(dialog);
            });

            // Always return True when a Notification is shown
            return true;
        }

        public bool ShowCustom(string title, object datacontext, int width, int height, bool canResize, bool autoSize, bool showTitle, bool showCancelButton, string okText, string cancelText, Func<Task<bool>> callback)
        {
            bool returnValue = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var dialog = new CustomDialog(title: title, datacontext: datacontext, width: width, height: height, canResize: canResize, autoSize: autoSize, showTitle: showTitle, showCancelButton: showCancelButton, okText: okText, cancelText: cancelText, callback: callback);

                if (dialog != null)
                {
                    this.ShowDialog(dialog);

                    if (dialog.DialogResult.HasValue & dialog.DialogResult.Value)
                    {
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
            });

            return returnValue;
        }

        public bool ShowConfirmation(string title, string content, string okText, string cancelText)
        {
            bool returnValue = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                ConfirmationDialog dialog = new ConfirmationDialog(title: title, content: content, okText: okText, cancelText: cancelText);
                this.ShowDialog(dialog);

                if (dialog.DialogResult.HasValue & dialog.DialogResult.Value)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            });

            return returnValue;
        }
    }
}
