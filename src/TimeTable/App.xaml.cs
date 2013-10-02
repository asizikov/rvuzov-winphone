using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TimeTable.IoC;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using System;
using System.Linq;

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
            Bootstrapper.InitApplication(RootFrame);

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
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

        private static void CommonActivated()
        {
            var flurryPublisher = ContainerInstance.Current.Resolve<FlurryPublisher>();
            var cache = ContainerInstance.Current.Resolve<ICache>();

            cache.PullFromStorage();
            flurryPublisher.StartSession("secret key"); //change to normal key, after we got flurry lib
        }

        private static void CommonDeactivated()
        {
            var flurryPublisher = ContainerInstance.Current.Resolve<FlurryPublisher>();
            var cache = ContainerInstance.Current.Resolve<ICache>();

            flurryPublisher.EndSession();
            cache.PushToStorage();
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
            RootFrame = new PhoneApplicationFrame();
            RootFrame.UriMapper = (UriMapper) Resources["ApplicationUriMapper"];
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            // Setting new tiles
            ShellTile appTile = ShellTile.ActiveTiles.First();
            bool isTargetedVersion = Environment.OSVersion.Version >= new Version(7, 10, 8858);
            if (isTargetedVersion)
            {
                // Get the new FlipTileData type.
                Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                // Loop through any existing Tiles that are pinned to Start.
                var tileToUpdate = ShellTile.ActiveTiles.First();

                // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                // Set the properties. 
                SetProperty(UpdateTileData, "Title", "");
                SetProperty(UpdateTileData, "SmallBackgroundImage", new Uri("SmallTile.png", UriKind.Relative));
                SetProperty(UpdateTileData, "BackgroundImage", new Uri("MediumTile.png", UriKind.Relative));
                SetProperty(UpdateTileData, "WideBackgroundImage", new Uri("WideTile.png", UriKind.Relative));

                // Invoke the new version of ShellTile.Update.
                shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });


            }
            else
            {
                StandardTileData newTile = new StandardTileData
                {
                    Title = "",
                    BackgroundImage = new Uri("MediumTile.png", UriKind.Relative),
                };
                appTile.Update(newTile);
            }
        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
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