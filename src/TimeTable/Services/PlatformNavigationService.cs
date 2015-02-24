using System;
using System.Windows;
using Microsoft.Phone.Controls;
using TimeTable.Mvvm.Navigation;

namespace TimeTable.Services
{
    public class PlatformNavigationService : IPlatformNavigationService
    {
        private PhoneApplicationFrame PhoneApplicationFrame { get; set; }

        public PlatformNavigationService()
        {
            PhoneApplicationFrame = ((App) Application.Current).RootFrame;
        }

        public void Navigate(Uri path)
        {
            PhoneApplicationFrame.Navigate(path);
        }
    }
}