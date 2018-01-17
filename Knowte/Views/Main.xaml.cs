using Digimezzo.Utilities.Utils;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Knowte.Views
{
    public partial class Main : Page
    {
        private Notes.Notes notesPage;
        private Settings.Settings settingsPage;
        private Information.Information informationPage;

        public Main()
        {
            InitializeComponent();
            this.MySplitView.PaneClosed += (_, __) => this.HideOverlayAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.NotesButton.IsChecked = true; // Default menu item
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            this.MySplitView.IsPaneOpen = !this.MySplitView.IsPaneOpen;
            if (this.MySplitView.IsPaneOpen) this.ShowOverlay();
        }

        private void CloseSplitViewPane()
        {
            this.MySplitView.IsPaneOpen = false;
            this.HideOverlayAsync();
        }

        private void ShowOverlay()
        {
            Storyboard showOverlayStoryboard = this.FindResource("ShowOverlayStoryboard") as Storyboard;
            showOverlayStoryboard.Begin();
            this.Overlay.Visibility = Visibility.Visible;
        }

        private async void HideOverlayAsync()
        {
            Storyboard hideOverlayStoryboard = this.FindResource("HideOverlayStoryboard") as Storyboard;
            hideOverlayStoryboard.Begin();

            await Task.Delay(250); // Make sure the storyboard has finished before collapsing the overlay
            this.Overlay.Visibility = Visibility.Collapsed;
        }

        private void NotesButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PageTitle.Text = ResourceUtils.GetString("Language_Notes");

            if (this.notesPage == null)
            {
                this.notesPage = new Notes.Notes();
            }

            this.MainFrame.Navigate(this.notesPage);
            this.CloseSplitViewPane();
        }

        private void SettingsButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PageTitle.Text = ResourceUtils.GetString("Language_Settings");

            if (this.settingsPage == null)
            {
                this.settingsPage = new Settings.Settings();
            }

            this.MainFrame.Navigate(this.settingsPage);
            this.CloseSplitViewPane();
        }

        private void InformationButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PageTitle.Text = ResourceUtils.GetString("Language_Information");

            if (this.informationPage == null)
            {
                this.informationPage = new Information.Information();
            }

            this.MainFrame.Navigate(this.informationPage);
            this.CloseSplitViewPane();
        }
    }
}
