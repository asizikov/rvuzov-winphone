using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Updates
    {
        [JsonProperty("last_updated")]
        public int Last { get; set; }
    }
}