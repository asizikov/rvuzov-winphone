using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace TimeTable.ViewModel.Data
{
    internal class CacheItem
    {
        public object Data { get; set; }
        public string Url { get; set; }
        public Type Type { get; set; }
    }

    public class InMemoryCache : ICache
    //todo: thread safe!
    {
        [NotNull]
        private readonly Dictionary<string, CacheItem> cache = new Dictionary<string, CacheItem>();

        public bool IsCached<T>(string url) where T : new()
        {
            //todo: thread safe!
            return cache.ContainsKey(url);
        }

        public void Put<T>(T item, string url)
        {
            var cacheItem = new CacheItem
            {
                Url = url,
                Type = typeof(T),
                Data = item
            };
            if (cache.ContainsKey(url))
            {
                cache[url] = cacheItem;
            }
            else
            {
                cache.Add(url, cacheItem);
            }
        }

        public T Fetch<T>(string url)
        {
            if (!cache.ContainsKey(url)) throw new ArgumentException("Requested item is not cached");

            var item = cache[url];
            return (T)item.Data;
        }
    }
}