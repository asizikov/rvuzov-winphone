using System;
using System.Windows.Controls;
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
        Uri GetUri<TView, TData>(TData data) where TView : Page;

        [PublicAPI]
        Uri GetUri<TView>() where TView : Page;
    }
}