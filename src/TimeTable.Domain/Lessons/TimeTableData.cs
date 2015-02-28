using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Domain.Lessons
{
    [UsedImplicitly]
    public sealed class TimeTableData
    {
        [JsonProperty("parity_countdown")]
        public long ParityCountdown { get; set; }
        [JsonProperty("days")]
        public List<Day> Days { get; set; }

        [JsonProperty("last_updated")]
        public long LastUpdated { get; set; }
    }
}