using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TinyIoC;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        public static BaseViewModel GetTmpViewModel()
        {
            return new TmpViewModel(new AsyncDataProvider(), TinyIoCContainer.Current.Resolve<INavigationService>());
        }
    }
}
