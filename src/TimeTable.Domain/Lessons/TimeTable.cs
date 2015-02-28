using Newtonsoft.Json;

namespace TimeTable.Domain.Lessons
{
    public sealed class TimeTable : IUpdatableModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public TimeTableData Data { get; set; }

        public long LastUpdated
        {
            get
            {
                return Data != null ? Data.LastUpdated : 0;
            }
        }
    }
}