using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Domain.Lessons
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class LessonTeacher
    {
        [JsonProperty("teacher_name")]
        public string Name { get; set; }

        [JsonProperty("teacher_id")]
        public string Id { get; set; }
    }
}