using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Model;

namespace TimeTable.ViewModel.Services
{
    public class DefaultUniversityAndGroupManager
    {
        [NotNull] private const string DEFAULTPARAMS = "Defaultparams";
        [NotNull] private DefaultParams _defaultParams;
        public string defaultUniversity {get; set;}
        public string defaultGroup { get; set; }


        public DefaultUniversityAndGroupManager()
        {
            LoadDefaultParams();
        }
        public DefaultUniversityAndGroupManager(string universityName, string groupName)
        {
            Add(universityName, groupName);
            LoadDefaultParams();
        }
        [NotNull]
        private void LoadDefaultParams()
        {
            DefaultParams defParams;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(DEFAULTPARAMS))
            {
                defParams = _defaultParams;
                IsolatedStorageSettings.ApplicationSettings.Add(DEFAULTPARAMS, SerializeToStrng(defParams));
            }
            else
            {
                var defParamsJsonString = (string)IsolatedStorageSettings.ApplicationSettings[DEFAULTPARAMS];
                defParams = DeserializeFromString(defParamsJsonString);
            }
            defaultGroup = defParams.DefaultGroup;
            defaultUniversity = defParams.DefaultUniversity;
        }

        [NotNull, Pure]
        private DefaultParams DeserializeFromString(string favsJsonString)
        {
            var deserializedFavs = JsonConvert.DeserializeObject<DefaultParams>(favsJsonString);
            return deserializedFavs;
        }

        private string SerializeToStrng(DefaultParams defParams)
        {
            return JsonConvert.SerializeObject(defParams);
        }

        public void Add(string universityName, string groupName)
        {
            var newItem = new DefaultParams
            {
                DefaultGroup = groupName,
                DefaultUniversity = universityName

            };
            _defaultParams = newItem;
        }
    }
}
