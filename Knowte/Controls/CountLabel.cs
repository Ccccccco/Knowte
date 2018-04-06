using System.Windows;
using System.Windows.Controls;

namespace Knowte.Controls
{
    public class CountLabel : Label
    {
        static CountLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CountLabel), new FrameworkPropertyMetadata(typeof(CountLabel)));
        }
    }
}
