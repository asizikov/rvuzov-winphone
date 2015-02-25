using System;

namespace TimeTable.Mvvm.Navigation
{
    public interface IPlatformNavigationService
    {
        void Navigate(Uri path);
        bool CanGoBack();
        void RemoveBackEntry();
    }
}