using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly]
    public sealed class TimeTableData
    {
        [JsonProperty("parity_countdown")]
        public long ParityCountdown { get; set; }
        [JsonProperty("days")]
        public List<Day> Days { get; set; }
    }
}