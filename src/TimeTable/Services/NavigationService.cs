﻿using System;
using System.Collections.Generic;
using System.Text;
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

        public void GoToPage([NotNull] string pageName, IEnumerable<NavigationParameter> parameters = null)
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
                    sb.Append(navigationParameter.Value);
                    sb.Append("&");
                }
            }
            _rootFrame.Navigate(new Uri(sb.ToString(), UriKind.Relative));
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
