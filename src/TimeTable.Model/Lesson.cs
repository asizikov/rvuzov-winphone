using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public sealed class Lesson
    {
        [JsonProperty("teachers")]
        public List<LessonTeacher> Teachers { get; set; }

        [JsonProperty("parity")]
        public int Parity { get; set; }

        [JsonProperty("time_start")]
        public string TimeStart { get; set; }

        [JsonProperty("time_end")]
        public string TimeEnd { get; set; }

        [JsonProperty("lesson_id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }

        [JsonProperty("auditories")]
        public List<Auditory> Auditories { get; set; }

        [JsonProperty("date_start")]
        public string DateStart { get; set; }

        [JsonProperty("date_end")]
        public string DateEnd { get; set; }

        [JsonProperty("dates")]
        public List<object> Dates { get; set; }
    }

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