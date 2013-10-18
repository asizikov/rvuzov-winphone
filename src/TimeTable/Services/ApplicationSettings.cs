using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Model.Internal;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public class ApplicationSettings : BaseApplicationSettings
    {
        private const string KEY = "Settings";

        public ApplicationSettings()
        {
            Me = Settings();
        }


        [NotNull, Pure]
        private static Me Settings()
        {
            Me settings;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(KEY))
            {
                settings = GetEmptySettings();
                IsolatedStorageSettings.ApplicationSettings.Add(KEY, SerializeToStrng(settings));
            }
            else
            {
                var favsJsonString = (string)IsolatedStorageSettings.ApplicationSettings[KEY];
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

        private static string SerializeToStrng(Me favs)
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
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(KEY))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(KEY, SerializeToStrng(Me));
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[KEY] = SerializeToStrng(Me);
            }
        }
    }
}