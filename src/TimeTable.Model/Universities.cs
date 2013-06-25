using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class Universities: IUpdatableModel
    {
        [JsonProperty("Last")]
        public int LastUpdated { get; set; }

        [JsonProperty("universities")]
        public List<University> UniversitesList { get; set; }
    }

    public class University
    {
        [JsonProperty("university_id")]
        public int Id { get; set; }

        [JsonProperty("university_name")]
        public string Name { get; set; }

        [JsonProperty("university_shortname")]
        public string ShortName { get; set; }

        [JsonProperty("parity_countdown")]
        public string ParityCountdown { get; set; }
    }
}