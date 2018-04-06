using System.Windows;
using System.Windows.Controls;

namespace Knowte.Controls
{
    public class NotesPivotItemHeader : Control
    {
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(NotesPivotItemHeader), new PropertyMetadata(false));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(NotesPivotItemHeader), new PropertyMetadata(null));

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register(nameof(Count), typeof(int), typeof(NotesPivotItemHeader), new PropertyMetadata(0));

        static NotesPivotItemHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotesPivotItemHeader), new FrameworkPropertyMetadata(typeof(NotesPivotItemHeader)));
        }
    }
}
