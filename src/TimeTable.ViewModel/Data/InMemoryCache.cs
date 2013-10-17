using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.ViewModel.Data
{
    public class CacheItem
    {
        public object Data { get; set; }
        public string Url { private get; set; }
        public Type Type { private get; set; }
    }

    public class InMemoryCache : ICache
        //todo: thread safe!
    {
        [NotNull] private Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        [NotNull] private readonly IsolatedStorageFile _storageFile = IsolatedStorageFile.GetUserStoreForApplication();
        [NotNull] private readonly object _readLock = new object();

        private const string STORAGE_FILE_NAME = "Cache";

        public bool IsCached<T>(string url) where T : new()
        {
            //todo: thread safe!
            return _cache.ContainsKey(url);
        }

        public void Put<T>(T item, string url) where T : new()
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

        public T Fetch<T>(string url) where T : new()
        {
            if (!_cache.ContainsKey(url)) throw new ArgumentException("Requested item is not cached");

            var item = _cache[url];

            if (item.Data is T)
            {
                return (T) item.Data;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(item.Data.ToString());
            }
        }

        public void PushToStorage()
        {
            lock (_readLock)
            {
                using (var storageFileStream = _storageFile.CreateFile(STORAGE_FILE_NAME))
                {
                    WriteFile(storageFileStream);
                }
            }
        }

        public void PullFromStorage()
        {
            lock (_readLock)
            {
                if (_storageFile.FileExists(STORAGE_FILE_NAME))
                {
                    using (var storageFileStream = _storageFile.OpenFile(STORAGE_FILE_NAME, FileMode.Open))
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
                json.Serialize(writer, _cache, _cache.GetType());
            }
        }

        private Dictionary<string, CacheItem> ReadFile(Stream fileStream)
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