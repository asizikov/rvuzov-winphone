using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using JetBrains.Annotations;
using TimeTable.Mvvm.Navigation.Serialization;

namespace TimeTable.Mvvm.Navigation
{
    public class NavigationService : INavigationService
    {
        private ISerializer Serializer { get; set; }
        private INavigationUriProvider NavigationUriProvider { get; set; }
        private IPlatformNavigationService PlatformNavigationService { get; set; }
        private string Key { get; set; }

        public NavigationService(IPlatformNavigationService platformNavigationService)
            : this(new NavigationServiceConfiguration(), platformNavigationService)
        {
        }

        public NavigationService([NotNull] NavigationServiceConfiguration configuration,
                                 [NotNull] IPlatformNavigationService platformNavigationService)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (platformNavigationService == null) throw new ArgumentNullException("platformNavigationService");

            Serializer = configuration.Serializer;
            NavigationUriProvider = configuration.NavigationUriProvider;
            PlatformNavigationService = platformNavigationService;
            Key = "NavigationContext";
            Debug.WriteLine("NavigationService::Ctor");
        }

        [PublicAPI]
        public void NavigateTo<TViewModel>() where TViewModel : PageViewModel
        {
            var path = GetUriInternal<TViewModel>();
            NavigateInternal(path);
        }

        public void NavigateTo<TViewModel>(int removeFromStack) where TViewModel : PageViewModel
        {
            NavigateTo<TViewModel>();
            SmartDispatcher.BeginInvoke(() => RemoveEntries(removeFromStack));
        }

        [PublicAPI]
        public void NavigateTo<TViewModel, TData>(TData data) where TViewModel : PageViewModel<TData>
        {
            var path = GetUriInternal<TViewModel, TData>(data);
            NavigateInternal(path);
        }

        public void NavigateTo<TViewModel, TData>(TData data, int removeFromStack)
            where TViewModel : PageViewModel<TData>
        {
            NavigateTo<TViewModel, TData>(data);
            SmartDispatcher.BeginInvoke(() => RemoveEntries(removeFromStack));
        }

        private Uri GetUriInternal<TViewModel, TData>(TData data) where TViewModel : PageViewModel<TData>
        {
            var uri = NavigationUriProvider.Get<TViewModel>();
            var navigationContext = NavigationContext.Create(uri.OriginalString, data);
            var navigationEvent = BuildNavigationEvent(navigationContext, uri);
            return BuildPath(navigationEvent);
        }

        private Uri BuildPath(NavigationEvent navigationEvent)
        {
            var uriString = string.Format("{0}?{1}={2}", navigationEvent.Destination.OriginalString, Key,
                HttpUtility.UrlEncode(navigationEvent.Context));
            var path = new Uri(uriString, UriKind.Relative);
            return path;
        }

        private Uri GetUriInternal<TViewModel>() where TViewModel : PageViewModel
        {
            var uri = NavigationUriProvider.Get<TViewModel>();
            var navigationContext = NavigationContext.Create(uri.OriginalString);
            var navigationEvent = BuildNavigationEvent(navigationContext, uri);
            return BuildPath(navigationEvent);
        }

        public Uri GetUri<TView, TData>(TData data) where TView : Page
        {
            var uri = typeof (TView).GetUri();
            var navigationContext = NavigationContext.Create(uri.OriginalString, data);
            var navigationEvent = BuildNavigationEvent(navigationContext, uri);
            return BuildPath(navigationEvent);
        }

        public Uri GetUri<TView>() where TView : Page
        {
            var uri = typeof (TView).GetUri();
            var navigationContext = NavigationContext.Create(uri.OriginalString);
            var navigationEvent = BuildNavigationEvent(navigationContext, uri);
            return BuildPath(navigationEvent);
        }

        private NavigationEvent BuildNavigationEvent<TData>(NavigationContext<TData> navigationContext, Uri uri)
        {
            var serializedContext = Serializer.Serialize(navigationContext);
            Debug.WriteLine("NavigationService::serialized context " + serializedContext);
            var encoded = Base64Encode(serializedContext);

            var navigationEvent = new NavigationEvent
            {
                Context = encoded,
                Destination = uri
            };
            return navigationEvent;
        }

        private NavigationEvent BuildNavigationEvent(NavigationContext navigationContext, Uri uri)
        {
            var serializedContext = Serializer.Serialize(navigationContext);
            var encoded = Base64Encode(serializedContext);

            var navigationEvent = new NavigationEvent
            {
                Context = encoded,
                Destination = uri
            };
            return navigationEvent;
        }

        private void NavigateInternal(Uri path)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                Debug.WriteLine("NavigationService::Navigating to {0}", path);
                PlatformNavigationService.Navigate(path);
            });
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private void RemoveEntries(int numberOfItemsToRemove)
        {
            for (var counter = 0; counter < numberOfItemsToRemove; counter++)
            {
                if (PlatformNavigationService.CanGoBack())
                {
                    PlatformNavigationService.RemoveBackEntry();
                }
                else
                {
                    return;
                }
            }
        }
    }
}