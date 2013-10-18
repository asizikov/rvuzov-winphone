using TimeTable.Model;

namespace TimeTable.ViewModel.Data
{
    public interface IWebCache
    {
        bool IsCached<T>(string url) where T : new();
        void Put<T>(T item, string url) where T : new();
        T Fetch<T>(string url) where T : new ();
        void PullFromStorage();
        void PushToStorage();
    }
}