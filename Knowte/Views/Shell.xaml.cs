using Digimezzo.Utilities.Settings;
using Digimezzo.WPFControls;
using Knowte.Core.Prism;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Knowte.Views
{
    public partial class Shell : BorderlessWindows10Window
    {
        private IEventAggregator eventAggregator;

        public Shell(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<NavigateToMain>().Subscribe((_) => this.ShowMain());
        }

        private void Shell_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SettingsClient.Get<bool>("Configuration", "ShowWelcome"))
            {
                SettingsClient.Set<bool>("Configuration", "ShowWelcome", false);
                this.ShowWelcome();
            }
            else
            {
                this.ShowMain();
            }
        }

        private void ShowMain()
        {
            this.ShowMaxRestoreButton = true;
            this.ShowMinButton = true;
            this.ShellFrame.Navigate(new Main());
        }

        private void ShowWelcome()
        {
            this.ShowMaxRestoreButton = false;
            this.ShowMinButton = false;
            this.ShellFrame.Navigate(new Welcome());
        }

        private void ShellFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            var ta = new ThicknessAnimation();
            ta.Duration = TimeSpan.FromSeconds(0.3);
            ta.DecelerationRatio = 0.7;
            ta.To = new Thickness(0, 0, 0, 0);
            if (e.NavigationMode == NavigationMode.New)
            {
                ta.From = new Thickness(500, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ta.From = new Thickness(0, 0, 500, 0);
            }

            (e.Content as Page).BeginAnimation(MarginProperty, ta);
        }

        private void WelcomeCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
