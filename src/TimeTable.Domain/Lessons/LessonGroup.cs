using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Domain.Lessons
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class LessonGroup
    {
        [JsonProperty("group_id")]
        public int Id { get; set; }

        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }
}