using Knowte.Core.Base;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Knowte.Presentation.Controls
{
    public partial class FullLogo : UserControl
    {
        public Brush IconForeground
        {
            get { return (Brush)GetValue(IconForegroundProperty); }
            set { SetValue(IconForegroundProperty, value); }
        }

        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Brush), typeof(FullLogo), new PropertyMetadata(null));

        public Brush IconBackground
        {
            get { return (Brush)GetValue(IconBackgroundProperty); }
            set { SetValue(IconBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register("IconBackground", typeof(Brush), typeof(FullLogo), new PropertyMetadata(null));

        public bool ShowApplicationName
        {
            get { return (bool)GetValue(ShowApplicationNameProperty); }
            set { SetValue(ShowApplicationNameProperty, value); }
        }

        public static readonly DependencyProperty ShowApplicationNameProperty =
            DependencyProperty.Register("ShowApplicationName", typeof(bool), typeof(FullLogo), new PropertyMetadata(true));

        public string ApplicationName => ProductInformation.ApplicationName.ToUpper();

        public FullLogo()
        {
            InitializeComponent();
        }
    }
}
