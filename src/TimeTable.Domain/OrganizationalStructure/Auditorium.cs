using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Domain.OrganizationalStructure
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class Auditorium
    {
        [JsonProperty("auditory_id")]
        public int Id { get; set; }

        [CanBeNull, JsonProperty("auditory_name")]
        public string Name { get; set; }

        [CanBeNull, JsonProperty("auditory_address")]
        public string Address { get; set; }
    }
}