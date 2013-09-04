using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class TimeTable : IUpdatableModel
    {
        [JsonProperty("last_updated")]
        public int LastUpdated { get; set; }
        [JsonProperty("parity_countdown")]
        public string ParityCountdown { get; set; }
        [JsonProperty("days")]
        public List<Day> Days { get; set; }
    }
}