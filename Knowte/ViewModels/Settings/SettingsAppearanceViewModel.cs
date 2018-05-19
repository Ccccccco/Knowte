using Digimezzo.Foundation.Core.Settings;
using Knowte.Core.Base;
using Knowte.Core.IO;
using Knowte.Services.Appearance;
using Knowte.Services.I18n;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Knowte.ViewModels.Settings
{
    public class SettingsAppearanceViewModel : BindableBase
    {
        private bool followWindowsColor;
        private IAppearanceService appearanceService;
        private II18nService i18nService;
        private ObservableCollection<ColorScheme> colorSchemes = new ObservableCollection<ColorScheme>();
        private ColorScheme selectedColorScheme;
        private ObservableCollection<Language> languages = new ObservableCollection<Language>();
        private Language selectedLanguage;
        private bool sortNotesByModificationDate;

        public DelegateCommand AddColorCommand { get; set; }

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

        public bool SortNotesByModificationDate
        {
            get { return this.sortNotesByModificationDate; }

            set
            {
                SettingsClient.Set<bool>("Appearance", "SortByModificationDate", value);
                SetProperty<bool>(ref this.sortNotesByModificationDate, value);
            }
        }

        public ObservableCollection<Language> Languages
        {
            get { return this.languages; }
            set { SetProperty<ObservableCollection<Language>>(ref this.languages, value); }
        }

        public Language SelectedLanguage
        {
            get { return this.selectedLanguage; }

            set
            {
                SetProperty<Language>(ref this.selectedLanguage, value);
                SettingsClient.Set<string>("Appearance", "Language", value.Code);
                this.ApplyLanguage(value.Code);
            }
        }

        public SettingsAppearanceViewModel(IAppearanceService appearanceService, II18nService i18nService)
        {
            this.appearanceService = appearanceService;
            this.i18nService = i18nService;

            this.GetLanguagesAsync();
            this.GetColorSchemesAsync();
            this.GetTogglesAsync();

            this.appearanceService.ColorSchemesChanged += ColorSchemesChangedHandler;
            this.i18nService.LanguagesChanged += (_, __) => this.GetLanguagesAsync();

            this.AddColorCommand = new DelegateCommand(() =>
            {
                SafeActions.TryOpenPath(Path.Combine(SettingsClient.ApplicationFolder(), ApplicationPaths.ColorSchemesDirectory));
            });
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

        private void ApplyLanguage(string languageCode)
        {
            Application.Current.Dispatcher.Invoke(() => this.i18nService.ApplyLanguageAsync(languageCode));
        }

        private async void GetLanguagesAsync()
        {
            List<Language> languagesList = this.i18nService.GetLanguages();

            ObservableCollection<Language> localLanguages = new ObservableCollection<Language>();

            await Task.Run(() =>
            {
                foreach (Language lang in languagesList)
                {
                    localLanguages.Add(lang);
                }
            });

            this.Languages = localLanguages;

            Language tempLanguage = null;

            await Task.Run(() =>
            {
                string savedLanguageCode = SettingsClient.Get<string>("Appearance", "Language");

                if (!string.IsNullOrEmpty(savedLanguageCode))
                {
                    tempLanguage = this.i18nService.GetLanguage(savedLanguageCode);
                }

                // If, for some reason, SelectedLanguage is Nothing (e.g. when the user 
                // deleted a previously existing language file), select the default language.
                if (tempLanguage == null)
                {
                    tempLanguage = this.i18nService.GetDefaultLanguage();
                }
            });

            // Only set SelectedLanguage when we are sure that it is not Nothing. Otherwise this could trigger strange 
            // behaviour in the setter of the SelectedLanguage Property (because the "value" would be Nothing)
            this.SelectedLanguage = tempLanguage;
        }
    }
}
