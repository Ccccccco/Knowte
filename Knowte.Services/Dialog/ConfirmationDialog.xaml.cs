using Digimezzo.Utilities.Utils;
using Digimezzo.WPFControls;
using System.Windows;

namespace Knowte.Services.Dialog
{
    public partial class ConfirmationDialog : BorderlessWindows10Window
    {
        public ConfirmationDialog(string title, string content, string okText, string cancelText) : base()
        {
            InitializeComponent();

            this.Title = title;
            this.TextBlockTitle.Text = title;
            this.TextBlockContent.Text = content;
            this.ButtonOK.Content = okText;
            this.ButtonCancel.Content = cancelText;

            WindowUtils.CenterWindow(this);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
