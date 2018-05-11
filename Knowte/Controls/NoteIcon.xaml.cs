using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Knowte.Controls
{
    public partial class NoteIcon : UserControl
    {
        public Brush IconForeground
        {
            get { return (Brush)GetValue(IconForegroundProperty); }
            set { SetValue(IconForegroundProperty, value); }
        }

        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Brush), typeof(NoteIcon), new PropertyMetadata(null));

        public Brush IconBackground
        {
            get { return (Brush)GetValue(IconBackgroundProperty); }
            set { SetValue(IconBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register("IconBackground", typeof(Brush), typeof(NoteIcon), new PropertyMetadata(null));

        public NoteIcon()
        {
            InitializeComponent();
        }
    }
}
