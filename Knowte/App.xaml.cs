using Digimezzo.Foundation.Core.IO;
using Digimezzo.Foundation.Core.Logging;
using Digimezzo.Foundation.Core.Settings;
using Digimezzo.Foundation.Core.Utils;
using Digimezzo.Foundation.WPF.Controls;
using Knowte.Core.Base;
using Knowte.Data;
using Knowte.Data.Repositories;
using Knowte.Services.App;
using Knowte.Services.Appearance;
using Knowte.Services.Collection;
using Knowte.Services.Dialog;
using Knowte.Services.I18n;
using Knowte.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Knowte
{
    public partial class App : PrismApplication
    {
        private Mutex instanceMutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Check that there is only one instance of the application running
            instanceMutex = new Mutex(true, string.Format("{0}-{1}", ProductInformation.ApplicationGuid, ProcessExecutable.AssemblyVersion().ToString()), out bool isNewInstance);

            // Process the command line arguments
            this.ProcessCommandLineArguments(isNewInstance);

            if (isNewInstance)
            {
                instanceMutex.ReleaseMutex();
                base.OnStartup(e);
            }
            else
            {
                LogClient.Warning("{0} is already running. Shutting down.", ProcessExecutable.Name());
                this.Shutdown();
            }
        }

        protected override void InitializeShell(Window shell)
        {
            LogClient.Info($"### STARTING {ProcessExecutable.Name()}, version {ProcessExecutable.AssemblyVersion()}, IsPortable = {SettingsClient.Get<bool>("Configuration", "IsPortable")}, Windows version = {EnvironmentUtils.GetFriendlyWindowsVersion()} ({EnvironmentUtils.GetInternalWindowsVersion()}) ###");

            // Handler for unhandled AppDomain exceptions
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var initializer = new Initializer();

            if (initializer.IsMigrationNeeded())
            {
                // Show the Update Window
                Windows10BorderlessWindow initWin = new Initialize();
                initWin.Show();
                initWin.ForceActivate();
            }

            Application.Current.MainWindow = shell;

            LogClient.Info("Showing Main screen");
            Application.Current.MainWindow.Show();
        }

        private void ProcessCommandLineArguments(bool isNewInstance)
        {
            // Get the command line arguments
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                LogClient.Info("Found command line arguments.");

                switch (args[1])
                {
                    case "/donate":
                        LogClient.Info("Detected DonateCommand from JumpList.");

                        try
                        {
                            Actions.TryOpenLink(args[2]);
                        }
                        catch (Exception ex)
                        {
                            LogClient.Error("Could not open the link {0} in Internet Explorer. Exception: {1}", args[2], ex.Message);
                        }
                        this.Shutdown();
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            // Log the exception and stop the application
            this.ExecuteEmergencyStop(ex);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Prevent default unhandled exception processing
            e.Handled = true;

            // Log the exception and stop the application
            this.ExecuteEmergencyStop(e.Exception);
        }

        private void ExecuteEmergencyStop(Exception ex)
        {
            LogClient.Error("Unhandled Exception. {0}", LogClient.GetAllExceptions(ex));

            // Close the application to prevent further problems
            LogClient.Info("### FORCED STOP of {0}, version {1} ###", ProcessExecutable.Name(), ProcessExecutable.AssemblyVersion().ToString());

            // Emergency save of the settings
            SettingsClient.Write();

            this.Shutdown();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterFactories();
            RegisterRepositories();
            RegisterServices();
            InitializeServices();
            RegisterViews();
            RegisterViewModels();

            void RegisterFactories()
            {
                containerRegistry.RegisterSingleton<ISQLiteConnectionFactory, SQLiteConnectionFactory>();
            }

            void RegisterRepositories()
            {
                containerRegistry.RegisterSingleton<ICollectionRepository, CollectionRepository>();
            }

            void RegisterServices()
            {
                containerRegistry.RegisterSingleton<IDialogService, DialogService>();
                containerRegistry.RegisterSingleton<IAppService, AppService>();
                containerRegistry.RegisterSingleton<II18nService, I18nService>();
                containerRegistry.RegisterSingleton<IAppearanceService, AppearanceService>();
                containerRegistry.RegisterSingleton<ICollectionService, CollectionService>();
            }

            void InitializeServices()
            {
                // Making sure resources are set before we need them
                Container.Resolve<II18nService>().ApplyLanguageAsync(SettingsClient.Get<string>("Configuration", "Language"));                                                                                                 
                Container.Resolve<IAppearanceService>().ApplyColorScheme(
                    SettingsClient.Get<string>("Appearance", "ColorScheme"),
                    SettingsClient.Get<bool>("Appearance", "FollowWindowsColor"));
            }

            void RegisterViews()
            {
                containerRegistry.Register<object, Shell>(typeof(Shell).FullName);
            }

            void RegisterViewModels()
            {
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }
    }
}
