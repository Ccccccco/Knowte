using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knowte.Services.Contracts.Appearance
{
    public interface IAppearanceService
    {
        List<ColorScheme> GetColorSchemes();

        ColorScheme GetColorScheme(string name);

        event EventHandler ColorSchemeChanged;

        event EventHandler ColorSchemesChanged;

        void ApplyColorScheme(string selectedColorScheme, bool followWindowsColor);

        void WatchWindowsColor(object window);

        void OnColorSchemeChanged(EventArgs e);

        void OnColorSchemesChanged(EventArgs e);

    }
}
