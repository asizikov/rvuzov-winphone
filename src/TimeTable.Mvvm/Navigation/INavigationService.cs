using System;
using JetBrains.Annotations;

namespace TimeTable.Mvvm.Navigation
{
    public interface INavigationService
    {
        [PublicAPI]
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;

        [PublicAPI]
        void NavigateTo<TViewModel>(int removeFromStack) where TViewModel : BaseViewModel;

        [PublicAPI]
        void NavigateTo<TViewModel, TData>(TData data) where TViewModel : BaseViewModel;

        [PublicAPI]
        void NavigateTo<TViewModel, TData>(TData data, int removeFromStack) where TViewModel : BaseViewModel;

        [PublicAPI]
        Uri GetUri<TViewModel, TData>(TData data) where TViewModel : BaseViewModel;
        [PublicAPI]

        Uri GetUri<TViewModel>() where TViewModel : BaseViewModel;
    }
}