using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class Teacher
    {
        [JsonProperty("teacher_name")]
        public string Name { get; set; }

        [JsonProperty("teacher_id")]
        public string Id { get; set; }

        [CanBeNull, JsonProperty("personalPage")]
        public string PersonalPage { get; set; }

        [CanBeNull, JsonProperty("additionalyInformation")]
        public string AdditionalyInformation { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}