using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel.Data
{
    internal class CacheItem
    {
        public object Data { get; set; }
        public string Url { private get; set; }
        public Type Type { private get; set; }
    }

    public class InMemoryCache : ICache
    //todo: thread safe!
    {
        [NotNull]
        private readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();

        public bool IsCached<T>(string url) where T : new()
        {
            //todo: thread safe!
            return _cache.ContainsKey(url);
        }

        public void Put<T>(T item, string url) where T : new ()
        {
            var cacheItem = new CacheItem
            {
                Url = url,
                Type = typeof(T),
                Data = item
            };
            if (_cache.ContainsKey(url))
            {
                _cache[url] = cacheItem;
            }
            else
            {
                _cache.Add(url, cacheItem);
            }
        }

        public T Fetch<T>(string url) where T : new ()
        {
            if (!_cache.ContainsKey(url)) throw new ArgumentException("Requested item is not cached");

            var item = _cache[url];
            return (T)item.Data;
        }
    }
}