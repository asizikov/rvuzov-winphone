using TimeTable.Networking;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        public static BaseViewModel GetTmpViewModel()
        {
            return new TmpViewModel(new WebService(), new RestfulCallFactory());
        }
    }
}
