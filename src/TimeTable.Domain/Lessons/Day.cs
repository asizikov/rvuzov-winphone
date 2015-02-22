using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Domain.Lessons
{
    [UsedImplicitly]
    public sealed class Day
    {
        [JsonProperty("lessons")]
        public List<Lesson> Lessons { get; set; }
        [JsonProperty("weekday")]
        public int Weekday { get; set; }
    }

    public sealed class ErrorMessage
    {
        [JsonProperty("lesson")]
        public ErrorLesson Lesson { get; set; }
        [JsonProperty("message")]
        public string ErrorText { get; set; }
    }

    public sealed class ErrorLesson
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
    }
}