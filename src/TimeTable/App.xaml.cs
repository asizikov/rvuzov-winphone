﻿using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TimeTable.Data.Cache;
using TimeTable.IoC;
using TimeTable.Networking.Cache;
using TimeTable.Services;
using TimeTable.ViewModel.FavoritedTimeTables;
using TimeTable.ViewModel.Services;

namespace TimeTable
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
            {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();
            ThemeManager.OverrideOptions = ThemeManagerOverrideOptions.SystemTrayColors;
            ThemeManager.ToDarkTheme();
            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Current.Host.Settings.EnableFrameRateCounter = true;
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
           CommonActivated();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            CommonActivated();
        }

        // Code to execute when the application is deactivated (sent to background)

        // This code will not execute when the application is closing

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            CommonDeactivated();
        }

        // Code to execute when the application is closing (eg, user hit Back)

        // This code will not execute when the application is deactivated

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            CommonDeactivated();
        }

        private void CommonActivated()
        {
            Bootstrapper.InitApplication(RootFrame);
            var flurryPublisher = Container.Resolve<FlurryPublisher>();
            ThreadPool.QueueUserWorkItem(o => flurryPublisher.StartSession());
        }

        private static void CommonDeactivated()
        {
            var flurryPublisher = Container.Resolve<FlurryPublisher>();
            var cache = Container.Resolve<IWebCache>();
            var favoritedItemsManager = Container.Resolve<FavoritedItemsManager>();
            var settings = Container.Resolve<BaseApplicationSettings>();
            var universitiesCache = Container.Resolve<UniversitiesCache>();
            flurryPublisher.EndSession();
            cache.PushToStorage();
            favoritedItemsManager.Save();
            settings.Save();
            universitiesCache.Save();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            var flurry = Container.Resolve<FlurryPublisher>();
            flurry.PublishException(e.ExceptionObject);
            flurry.EndSession();
            CrashLogger.SaveCrashInfo(e);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

 
            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame
            {
//                UriMapper = (UriMapper) Resources["ApplicationUriMapper"]
            };
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}