using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using JetBrains.Annotations;

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
        [NotNull] private readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        [NotNull] private readonly IsolatedStorageFile _storageFile = IsolatedStorageFile.GetUserStoreForApplication();

        private const string StorageFileName = "TimeTableData";

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
            return (T) item.Data;
        }

        public void PushToStorage()
        {
            if (_storageFile.FileExists(StorageFileName))
            {
                using (var storageFileStream = _storageFile.OpenFile(StorageFileName, FileMode.CreateNew))
                {
                    WriteFile(storageFileStream);
                }
            }
            else
            {
                using (var storageFileStream = _storageFile.CreateFile(StorageFileName))
                {
                    WriteFile(storageFileStream);
                }
            }
        }

        public void PullFromStorage()
        {
            if (_storageFile.FileExists(StorageFileName))
            {
                using (var storageFileStream = _storageFile.OpenFile(StorageFileName, FileMode.Open))
                {
                    ReadFile(storageFileStream);
                }
            }
        }

        private void WriteFile(Stream fileStream)
        {
            var xmlDoc = SerializeCacheDictionary();
            xmlDoc.Save(fileStream);
        }

        private void ReadFile(Stream fileStream)
        {
            var xmlDoc = XDocument.Load(fileStream);
            DeserializeCacheDictionary(xmlDoc);
        }

        private XDocument SerializeCacheDictionary()
        {
            var resultDoc = new XDocument("Cache");

            foreach (var cacheItem in _cache)
            {
                var xElement = new XElement("CacheItem");
                xElement.Add(new XAttribute(cacheItem.Key, cacheItem.Value));
            }

            return resultDoc;
        }

        private void DeserializeCacheDictionary(XDocument xmlDoc)
        {
            if (xmlDoc.Root == null || !xmlDoc.Root.HasElements)
                return;

            foreach (var element in xmlDoc.Root.Elements())
            {
                foreach (var attribute in element.Attributes())
                {
                    _cache.Add(attribute.Name.ToString(),
                               new CacheItem()
                                   {
                                       Data = attribute.Value,
                                   });
                }
            }
        }
    }
}