using System;

namespace TimeTable.ViewModel.Data
{
    public class CacheItem
    {
        public object Data { get; set; }
        public string Url { private get; set; }
        public Type Type { private get; set; }
    }
}