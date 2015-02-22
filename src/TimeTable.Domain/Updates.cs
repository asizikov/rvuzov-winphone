using Newtonsoft.Json;

namespace TimeTable.Domain
{
    public class Updates
    {
        [JsonProperty("last_updated")]
        public int Last { get; set; }
    }
}