using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Lesson
    {
        [JsonProperty("teachers")]
        public List<object> Teachers { get; set; }
        [JsonProperty("parity")]
        public int Parity { get; set; }
        [JsonProperty("auditory")]
        public string Auditory { get; set; }
        [JsonProperty("auditory_address")]
        public string AuditoryAddress { get; set; }
        [JsonProperty("time_start")]
        public string TimeStart { get; set; }
        [JsonProperty("time_end")]
        public string TimeEnd { get; set; }
        [JsonProperty("lesson_id")]
        public int Id { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}