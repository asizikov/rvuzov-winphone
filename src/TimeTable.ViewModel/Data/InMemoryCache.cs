using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Networking.Cache;

namespace TimeTable.ViewModel.Data
{
    public class InMemoryCache : IWebCache
    {
        [NotNull] private Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        [NotNull] private readonly IsolatedStorageFile _storageFile = IsolatedStorageFile.GetUserStoreForApplication();
        [NotNull] private readonly object _readLock = new object();

        private const string StorageFileName = "Cache";

        public bool IsCached<T>(string url) where T : class
        {
            return _cache.ContainsKey(url);
        }

        public void Put<T>(T item, string url) where T : class
        {
            var cacheItem = new CacheItem
            {
                Url = url,
                Type = typeof (T),
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

        public T Fetch<T>(string url) where T : class
        {
            if (!_cache.ContainsKey(url)) throw new ArgumentException("Requested item is not cached");

            var item = _cache[url];

            if (item.Data is T)
            {
                return (T) item.Data;
            }
            return JsonConvert.DeserializeObject<T>(item.Data.ToString());
        }

        public void PushToStorage()
        {
            lock (_readLock)
            {
                using (var storageFileStream = _storageFile.CreateFile(StorageFileName))
                {
                    WriteFile(storageFileStream);
                }
            }
        }

        public void PullFromStorage()
        {
            lock (_readLock)
            {
                if (_storageFile.FileExists(StorageFileName))
                {
                    using (var storageFileStream = _storageFile.OpenFile(StorageFileName, FileMode.Open))
                    {
                        _cache = new Dictionary<string, CacheItem>(ReadFile(storageFileStream));
                    }
                }
            }
        }

        private void WriteFile(Stream fileStream)
        {
            using (var writer = new StreamWriter(fileStream))
            {
                var json = new JsonSerializer();
                json.Serialize(writer, new Dictionary<string, CacheItem>(_cache), _cache.GetType());
            }
        }

        private static Dictionary<string, CacheItem> ReadFile(Stream fileStream)
        {
            using (var streamReader = new StreamReader(fileStream))
            {
                var json = new JsonSerializer();
                var reader = new JsonTextReader(streamReader);
                return json.Deserialize<Dictionary<string, CacheItem>>(reader);
            }
        }
    }
}