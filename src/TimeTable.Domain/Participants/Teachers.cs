using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Domain.Participants
{
    public class Teachers
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Teacher> TeachersList { get; set; }
    }
}
