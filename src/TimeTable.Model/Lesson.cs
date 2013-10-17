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
        public List<LessonGroup> Groups { get; set; }

        [JsonProperty("auditories")]
        public List<Auditorium> Auditoriums { get; set; }

        [JsonProperty("date_start")]
        public string DateStart { get; set; }

        [JsonProperty("date_end")]
        public string DateEnd { get; set; }

        [CanBeNull,JsonProperty("dates")]
        public List<string> Dates { get; set; }
    }
}