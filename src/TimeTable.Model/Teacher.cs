using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Teacher
    {
        [JsonProperty("teacher_name")]
        public string Name { get; set; }
        [JsonProperty("teacher_id")]
        public string Id { get; set; }
    }
}