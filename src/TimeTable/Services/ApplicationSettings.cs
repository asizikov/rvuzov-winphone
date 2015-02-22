using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Domain.Internal;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public class ApplicationSettings : BaseApplicationSettings
    {
        private const string Key = "Settings";

        public ApplicationSettings()
        {
            Me = Settings();
        }


        [NotNull, Pure]
        private static Me Settings()
        {
            Me settings;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Key))
            {
                settings = GetEmptySettings();
                IsolatedStorageSettings.ApplicationSettings.Add(Key, SerializeToString(settings));
            }
            else
            {
                var favsJsonString = (string)IsolatedStorageSettings.ApplicationSettings[Key];
                settings = DeserializeFromString(favsJsonString);
            }
            return settings;
        }

        [NotNull, Pure]
        private static Me DeserializeFromString(string favsJsonString)
        {
            var deserializedFavs = JsonConvert.DeserializeObject<Me>(favsJsonString);
            return deserializedFavs ?? GetEmptySettings();
        }

        private static string SerializeToString(Me favs)
        {
            return JsonConvert.SerializeObject(favs);
        }

        [NotNull, Pure]
        private static Me GetEmptySettings()
        {
            var favs = new Me();
            return favs;
        }

        public override void Save()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Key))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(Key, SerializeToString(Me));
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[Key] = SerializeToString(Me);
            }
        }
    }
}