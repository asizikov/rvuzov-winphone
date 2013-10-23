using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class Confirmation
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}