using Knowte.Core.Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace Knowte.ViewModels
{
    public class WelcomeViewModel : BindableBase
    {
        public enum WelcomePage
        {
            Start = 0,
            Finish = 1
        }

        private bool showStartButton;
        private bool showFinishButton;

        private IEventAggregator eventAggregator;

        private WelcomePage selectedWelcomePage;

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand GoForwardCommand { get; set; }
        public DelegateCommand FinishCommand { get; set; }

        public Int32 SelectedWelcomePageIndex
        {
            get { return (Int32)this.selectedWelcomePage; }
        }

        public bool ShowStartButton
        {
            get { return this.showStartButton; }
            set
            {
                SetProperty<bool>(ref this.showStartButton, value);
                RaisePropertyChanged(nameof(this.ShowNavigationButtons));
            }
        }

        public bool ShowFinishButton
        {
            get { return this.showFinishButton; }
            set
            {
                SetProperty<bool>(ref this.showFinishButton, value);
                RaisePropertyChanged(nameof(this.ShowNavigationButtons));
            }
        }

        public bool ShowNavigationButtons
        {
            get { return !this.showStartButton && !this.showFinishButton; }
        }

        public WelcomeViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.ShowStartButton = true;
            this.ShowFinishButton = false;

            this.SetSelectedWelcomePage(WelcomePage.Start);

            this.StartCommand = new DelegateCommand(() => this.GoForward());
            this.FinishCommand = new DelegateCommand(() => FinishWizard());
            this.GoBackCommand = new DelegateCommand(() => this.GoBack(), () => this.CanGoBack());
            this.GoForwardCommand = new DelegateCommand(() => this.GoForward(), () => this.CanGoForward());
        }

        private void SetSelectedWelcomePage(WelcomePage page)
        {
            this.selectedWelcomePage = page;
            RaisePropertyChanged(nameof(this.SelectedWelcomePageIndex));
        }

        private void FinishWizard()
        {
            this.eventAggregator.GetEvent<NavigateToMain>().Publish(null);
        }

        private void GoBack()
        {
            switch (this.selectedWelcomePage)
            {
                case WelcomePage.Finish:
                    this.SetSelectedWelcomePage(WelcomePage.Start);
                    break;
                default:
                    break;
            }

            this.GoBackCommand.RaiseCanExecuteChanged();
            this.GoForwardCommand.RaiseCanExecuteChanged();
        }

        private void GoForward()

        {
            // The first time we go forward is when we press start. 
            // So it is save to hide the start button here.
            this.ShowStartButton = false;

            switch (this.selectedWelcomePage)
            {
                case WelcomePage.Start:
                    this.SetSelectedWelcomePage(WelcomePage.Finish);
                    this.ShowFinishButton = true;
                    break;
                default:
                    break;
            }

            this.GoBackCommand.RaiseCanExecuteChanged();
            this.GoForwardCommand.RaiseCanExecuteChanged();
        }

        private bool CanGoBack()
        {
            return false;
        }

        private bool CanGoForward()
        {
            return true;
        }
    }
}
