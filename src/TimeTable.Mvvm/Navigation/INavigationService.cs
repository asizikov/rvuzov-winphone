using JetBrains.Annotations;

namespace TimeTable.Mvvm.Navigation
{
    public interface INavigationService
    {
        [PublicAPI]
        void NavigateTo<TViewModel>() where TViewModel : BaseViewModel;

        [PublicAPI]
        void NavigateTo<TViewModel, TData>(TData data) where TViewModel : BaseViewModel;
    }
}