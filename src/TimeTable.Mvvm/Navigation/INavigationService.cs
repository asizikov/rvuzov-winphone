using System;
using JetBrains.Annotations;

namespace TimeTable.Mvvm.Navigation
{
    public interface INavigationService
    {
        [PublicAPI]
        void NavigateTo<TViewModel>() where TViewModel : PageViewModel;

        [PublicAPI]
        void NavigateTo<TViewModel>(int removeFromStack) where TViewModel : PageViewModel;

        [PublicAPI]
        void NavigateTo<TViewModel, TData>(TData data) where TViewModel : PageViewModel<TData>;

        [PublicAPI]
        void NavigateTo<TViewModel, TData>(TData data, int removeFromStack) where TViewModel : PageViewModel<TData>;

        [PublicAPI]
        Uri GetUri<TViewModel, TData>(TData data) where TViewModel : PageViewModel<TData>;
        [PublicAPI]

        Uri GetUri<TViewModel>() where TViewModel : PageViewModel;
    }
}