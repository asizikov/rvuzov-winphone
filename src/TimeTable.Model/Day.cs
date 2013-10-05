using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    [UsedImplicitly]
    public sealed class Day
    {
        [JsonProperty("lessons")]
        public List<Lesson> Lessons { get; set; }
        [JsonProperty("weekday")]
        public int Weekday { get; set; }
    }
}