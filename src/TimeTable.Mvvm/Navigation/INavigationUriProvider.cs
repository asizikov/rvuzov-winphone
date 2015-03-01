using System;
using JetBrains.Annotations;

namespace TimeTable.Mvvm.Navigation
{
    public interface INavigationUriProvider
    {
        [NotNull]
        Uri Get<TViewModel>() where TViewModel : BaseViewModel;
    }
}