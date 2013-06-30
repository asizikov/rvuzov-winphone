using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Day
    {
        [JsonProperty("lessons")]
        public List<Lesson> Lessons { get; set; }
        [JsonProperty("weekday")]
        public int Weekday { get; set; }
    }
}