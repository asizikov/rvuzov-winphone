using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{

    public interface IUpdatableModel
    {
        int LastUpdated { get; set; }
    }

    public sealed class UniversitesAll: IUpdatableModel
    {
        [JsonProperty("last_updated")]
        public int LastUpdated { get; set; }

        [JsonProperty("universities")]
        public List<University> Universities { get; set; }
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

    public class LastUpdated
    {
        public int last_updated { get; set; }
    }
}