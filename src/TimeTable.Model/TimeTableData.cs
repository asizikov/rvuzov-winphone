using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly]
    public sealed class TimeTableData : IUpdatableModel
    {
        [JsonProperty("parity_countdown")]
        public string ParityCountdown { get; set; }
        [JsonProperty("last_updated")]
        public int LastUpdated { get; set; }
        [JsonProperty("days")]
        public List<Day> Days { get; set; }
    }
}