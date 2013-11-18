using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class Teacher
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [CanBeNull, JsonProperty("personalPage")]
        public string PersonalPage { get; set; }

        [CanBeNull, JsonProperty("additionalyInformation")]
        public string AdditionalyInformation { get; set; }
    }
}