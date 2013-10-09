using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class Auditory
    {
        [JsonProperty("auditory_id")]
        public int Id { get; set; }

        [CanBeNull, JsonProperty("auditory_name")]
        public string Name { get; set; }

        [CanBeNull, JsonProperty("auditory_address")]
        public string Address { get; set; }
    }
}