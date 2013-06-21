namespace TimeTable.ViewModel.Data
{
    public interface ICache
    {
        bool IsCached<T>(string url) where T : new();
        void Put<T>(T item, string url);
        T Fetch<T>(string url);
    }
}