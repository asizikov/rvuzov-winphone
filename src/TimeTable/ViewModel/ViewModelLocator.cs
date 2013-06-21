using TimeTable.ViewModel.Data;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        public static BaseViewModel GetTmpViewModel()
        {
            return new TmpViewModel(new AsyncDataProvider());
        }
    }
}
