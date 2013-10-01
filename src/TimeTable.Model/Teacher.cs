using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Teacher
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [CanBeNull, JsonProperty("personalPage")]
        public string PersonalPage { get; set; }

        [CanBeNull, JsonProperty("additionalyInformation")]
        public string AdditionalyInformation { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}