using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class LessonGroup
    {
        [JsonProperty("group_id")]
        public string Id { get; set; }

        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }
}