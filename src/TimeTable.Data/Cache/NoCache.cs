using TimeTable.Networking.Cache;

namespace TimeTable.Data.Cache
{
    public sealed class NoCache : IWebCache
    {
        public bool IsCached<T>(string url) where T : class
        {
            return false;
        }

        public void Put<T>(T item, string url) where T : class
        {
        }

        public T Fetch<T>(string url) where T : class
        {
            return default(T);
        }

        public void PullFromStorage()
        {
        }

        public void PushToStorage()
        {
        }
    }
}