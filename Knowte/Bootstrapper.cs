using Digimezzo.Foundation.Core.Logging;
using Digimezzo.Foundation.Core.Settings;
using Knowte.Core.Extensions;
using Knowte.Data;
using Knowte.Data.Contracts;
using Knowte.Data.Contracts.Repositories;
using Knowte.Data.Repositories;
using Knowte.Services.Appearance;
using Knowte.Services.Collection;
using Knowte.Services.Constracts.Dialog;
using Knowte.Services.Constracts.I18n;
using Knowte.Services.Contracts.Appearance;
using Knowte.Services.Contracts.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.I18n;
using Knowte.Views;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace Knowte
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.RegisterFactories();
            this.RegisterRepositories();
            this.RegisterServices();
            this.InitializeServices();
            this.RegisterViews();
            this.RegisterViewModels();

            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => { return Container.Resolve(type); });
        }

        private void RegisterFactories()
        {
            Container.RegisterSingletonType<ISQLiteConnectionFactory, SQLiteConnectionFactory>();
        }

        protected void RegisterRepositories()
        {
            Container.RegisterSingletonType<ICollectionRepository, CollectionRepository>();
        }

        private void RegisterServices()
        {
            Container.RegisterSingletonType<IDialogService, DialogService>();
            Container.RegisterSingletonType<II18nService, I18nService>();
            Container.RegisterSingletonType<IAppearanceService, AppearanceService>();
            Container.RegisterSingletonType<ICollectionService, CollectionService>();
            //Container.RegisterSingletonType<IJumpListService, JumpListService>();
        }

        private void InitializeServices()
        {
            // Making sure resources are set before we need them
            Container.Resolve<II18nService>().ApplyLanguageAsync(SettingsClient.Get<string>("Configuration", "Language")); // Set default language
            //Container.Resolve<IJumpListService>().RefreshJumpListAsync(); // Create the jump list
            Container.Resolve<IAppearanceService>().ApplyColorScheme(
                SettingsClient.Get<string>("Appearance", "ColorScheme"),
                SettingsClient.Get<bool>("Appearance", "FollowWindowsColor"));
        }

        protected void RegisterViews()
        {
            Container.RegisterType<object, Shell>(typeof(Shell).FullName);
        }

        protected void RegisterViewModels()
        {
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;

            LogClient.Info("Showing Main screen");
            Application.Current.MainWindow.Show();
        }
    }
}
