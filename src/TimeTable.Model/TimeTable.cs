using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class TimeTable: IUpdatableModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public TimeTableData Data { get; set; }

        public int LastUpdated
        {
            get
            {
                return Data != null ? Data.LastUpdated : int.MinValue;
            }
        }
    }
}