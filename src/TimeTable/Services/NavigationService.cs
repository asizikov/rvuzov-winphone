using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public class NavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public NavigationService([NotNull] PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null) throw new ArgumentNullException("rootFrame");
            _rootFrame = rootFrame;
        }

        public void GoToPage([NotNull] string pageName, [CanBeNull] IEnumerable<NavigationParameter> parameters = null)
        {
            if (pageName == null) throw new ArgumentNullException("pageName");

            var sb = new StringBuilder();
            sb.Append(pageName);
            if (parameters != null)
            {
                sb.Append("?");
                foreach (var navigationParameter in parameters)
                {
                    sb.Append(navigationParameter.Parameter);
                    sb.Append("=");
                    sb.Append(Uri.EscapeDataString(navigationParameter.Value));
                    sb.Append("&");
                }
            }
            SmartDispatcher.BeginInvoke(() => _rootFrame.Navigate(new Uri(sb.ToString(), UriKind.Relative)));
        }

        public void CleanNavigationStack()
        {
            while (_rootFrame.BackStack.Any())
            {
                _rootFrame.RemoveBackEntry();
            }
        }

        public void GoToPage(string page, IEnumerable<NavigationParameter> parameters, int numberOfItemsToRemove)
        {
            GoToPage(page, parameters);
            SmartDispatcher.BeginInvoke(() => RemoveEntries(numberOfItemsToRemove) );
        }

        private void RemoveEntries(int numberOfItemsToRemove)
        {
            for (var counter = 0; counter < numberOfItemsToRemove; counter++)
            {
                if (CanGoBack())
                {
                    _rootFrame.RemoveBackEntry();
                }
                else
                {
                    return;
                }
            }
        }

        public void GoBack()
        {
            _rootFrame.GoBack();
        }

        public bool CanGoBack()
        {
            return _rootFrame.CanGoBack;
        }
    }
}
