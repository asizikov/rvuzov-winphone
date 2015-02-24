using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Domain.Internal;

namespace TimeTable.ViewModel.Data
{
    public class DataWriter
    {
        [NotNull] private readonly IsolatedStorageFile _storageFile = IsolatedStorageFile.GetUserStoreForApplication();
        private const string STORAGE_FILE_NAME = "DB";
        [NotNull] private readonly object _readLock = new object();

        private static void WriteFile(Stream fileStream, Storage storage)
        {
            using (var writer = new StreamWriter(fileStream))
            {
                var json = new JsonSerializer();
                json.Serialize(writer, storage, storage.GetType());
            }
        }

        private Storage ReadFile(Stream fileStream)
        {
            using (var streamReader = new StreamReader(fileStream))
            {
                var json = new JsonSerializer();
                var reader = new JsonTextReader(streamReader);
                return json.Deserialize<Storage>(reader);
            }
        }

        public void Save(Storage storage)
        {
            lock (_readLock)
            {
                using (var storageFileStream = _storageFile.CreateFile(STORAGE_FILE_NAME))
                {
                    WriteFile(storageFileStream, storage);
                }
            }
        }

        public Storage LoadStorage()
        {
            lock (_readLock)
            {
                Storage favs;

                if (_storageFile.FileExists(STORAGE_FILE_NAME))
                {
                    using (var storageFileStream = _storageFile.OpenFile(STORAGE_FILE_NAME, FileMode.Open))
                    {
                        favs = ReadFile(storageFileStream);
                    }
                }
                else
                {
                    favs = GetEmptyStorage();
                }
                return favs;
            }
        }

        [NotNull, Pure]
        private static Storage GetEmptyStorage()
        {
            var storage = new Storage
            {
                Data = new List<UniversityItem>()
            };
            return storage;
        }
    }
}