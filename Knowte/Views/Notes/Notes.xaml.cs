using System;
using System.Windows;
using System.Windows.Controls;

namespace Knowte.Views.Notes
{
    public partial class Notes : Page
    {
        private double notebooksContainerMaximumWidth = 300;

        public double NotebooksContainerWidth
        {
            get { return (double)GetValue(NotebooksContainerWidthProperty); }
            set { SetValue(NotebooksContainerWidthProperty, value); }
        }

        public static readonly DependencyProperty NotebooksContainerWidthProperty =
            DependencyProperty.Register(nameof(NotebooksContainerWidth), typeof(double), typeof(Notes), new PropertyMetadata(null));

        public Notes()
        {
            InitializeComponent();
        }

        private void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.ActualWidth < 800)
                {
                    this.NotebooksContainerWidth = 0;
                    return;
                }
            }
            catch (Exception)
            {
                // Intentional suppression
            }

            this.NotebooksContainerWidth = this.notebooksContainerMaximumWidth;
        }
    }
}
