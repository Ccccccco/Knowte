using Digimezzo.Foundation.Core.Utils;
using Knowte.Core.Prism;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;

namespace Knowte.Views
{
    public partial class Main : Page
    {
        private Notes.Notes notesPage;
        private Settings.Settings settingsPage;
        private Information.Information informationPage;

        public Main(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<ManageCollections>().Subscribe((_) => this.MySplitView.IsPaneOpen = true);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NotesButton.IsChecked = true; // Default menu item
        }

        private void Navigate(object content)
        {
            this.MainFrame.Navigate(content);
            this.MySplitView.IsPaneOpen = false;
        }

        private void NotesButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PageTitle.Text = ResourceUtils.GetString("Language_Notes");

            if (this.notesPage == null)
            {
                this.notesPage = new Notes.Notes();
            }

            this.Navigate(this.notesPage);
        }

        private void SettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PageTitle.Text = ResourceUtils.GetString("Language_Settings");

            if (this.settingsPage == null)
            {
                this.settingsPage = new Settings.Settings();
            }

            this.Navigate(this.settingsPage);
        }

        private void InformationButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PageTitle.Text = ResourceUtils.GetString("Language_Information");

            if (this.informationPage == null)
            {
                this.informationPage = new Information.Information();
            }

            this.Navigate(this.informationPage);
        }
    }
}
