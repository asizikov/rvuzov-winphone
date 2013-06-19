using TimeTable.Networking;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        public static object GetTmpViewModel()
        {
            return new TmpViewModel(new WebService());
        }
    }
}
