using Funq;
using TimeTable.IoC;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        private static Container Container
        {
            get
            {
                return ContainerInstance.Current;
            }
        }

        private static TmpViewModel _tmpViewModel;

        public static BaseViewModel GetTmpViewModel()
        {
            return _tmpViewModel ??
                   (_tmpViewModel =
                       new TmpViewModel(Container.Resolve<INavigationService>(),
                           Container.Resolve<BaseApplicationSettings>(), new AsyncDataProvider()));
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(Container.Resolve<INavigationService>(), Container.Resolve<BaseApplicationSettings>());
        }

        public static BaseViewModel GetGroupstPageViewModel(int universityId)
        {
            return new GroupPageViewModel(Container.Resolve<INavigationService>(), Container.Resolve<BaseApplicationSettings>(), new AsyncDataProvider(), universityId);
        }
    }
}
