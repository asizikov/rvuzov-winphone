using System;

namespace TimeTable.Mvvm.Navigation
{
    public interface INavigationUriProvider
    {
        Uri Get<TViewModel>() where TViewModel : BaseViewModel;
    }
}