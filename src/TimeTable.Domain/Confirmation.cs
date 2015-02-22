using Newtonsoft.Json;

namespace TimeTable.Domain
{
    public sealed class Confirmation
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}