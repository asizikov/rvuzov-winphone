using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TinyIoC;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        private static TinyIoCContainer Container
        {
            get
            {
                return TinyIoCContainer.Current;
            }
        }

        public static BaseViewModel GetTmpViewModel()
        {
            return new TmpViewModel(new AsyncDataProvider(), Container.Resolve<INavigationService>());
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(Container.Resolve<INavigationService>());
        }
    }
}
