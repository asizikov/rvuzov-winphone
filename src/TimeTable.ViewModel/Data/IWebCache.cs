namespace TimeTable.ViewModel.Data
{
    public interface IWebCache
    {
        bool IsCached<T>(string url) where T : class;
        void Put<T>(T item, string url) where T : class;
        T Fetch<T>(string url) where T : class;
        void PullFromStorage();
        void PushToStorage();
    }
}