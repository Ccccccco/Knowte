using Digimezzo.Foundation.Core.Settings;
using Knowte.Services.Contracts.Appearance;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Knowte.ViewModels.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private bool followWindowsColor;
        private IAppearanceService appearanceService;
        private ObservableCollection<ColorScheme> colorSchemes = new ObservableCollection<ColorScheme>();
        private ColorScheme selectedColorScheme;

        public ObservableCollection<ColorScheme> ColorSchemes
        {
            get { return this.colorSchemes; }
            set { SetProperty<ObservableCollection<ColorScheme>>(ref this.colorSchemes, value); }
        }

        public ColorScheme SelectedColorScheme
        {
            get { return this.selectedColorScheme; }

            set
            {
                // value can be null when a ColorScheme is removed from the ColorSchemes directory
                if (value != null)
                {
                    SettingsClient.Set<string>("Appearance", "ColorScheme", value.Name);
                    this.ApplyColorScheme();
                }

                SetProperty<ColorScheme>(ref this.selectedColorScheme, value);
            }
        }

        public bool CanChooseColor
        {
            get { return !this.followWindowsColor; }
        }

        public bool FollowWindowsColor
        {
            get { return this.followWindowsColor; }

            set
            {
                SettingsClient.Set<bool>("Appearance", "FollowWindowsColor", value);
                this.ApplyColorScheme();
                SetProperty<bool>(ref this.followWindowsColor, value);
                this.RaisePropertyChanged(nameof(this.CanChooseColor));
            }
        }

        public SettingsViewModel(IAppearanceService appearanceService)
        {
            this.appearanceService = appearanceService;

            this.GetColorSchemesAsync();
            this.GetTogglesAsync();

            this.appearanceService.ColorSchemesChanged += ColorSchemesChangedHandler;
        }

        private void ColorSchemesChangedHandler(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => this.GetColorSchemesAsync());
        }

        private async void GetTogglesAsync()
        {
            await Task.Run(() =>
            {
                this.followWindowsColor = SettingsClient.Get<bool>("Appearance", "FollowWindowsColor");
            });
        }

        private async void GetColorSchemesAsync()
        {
            ObservableCollection<ColorScheme> localColorSchemes = new ObservableCollection<ColorScheme>();

            await Task.Run(() =>
            {
                foreach (ColorScheme cs in this.appearanceService.GetColorSchemes())
                {
                    localColorSchemes.Add(cs);
                }
            });

            this.ColorSchemes = localColorSchemes;

            string savedColorSchemeName = SettingsClient.Get<string>("Appearance", "ColorScheme");

            if (!string.IsNullOrEmpty(savedColorSchemeName))
            {
                this.SelectedColorScheme = this.appearanceService.GetColorScheme(savedColorSchemeName);
            }
            else
            {
                this.SelectedColorScheme = this.appearanceService.GetColorSchemes()[0];
            }
        }

        private void ApplyColorScheme()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.appearanceService.ApplyColorScheme(
                      SettingsClient.Get<string>("Appearance", "ColorScheme"),
                      SettingsClient.Get<bool>("Appearance", "FollowWindowsColor"));
            });
        }
    }
}
