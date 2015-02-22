using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Domain.People
{
    public class Teachers
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Teacher> TeachersList { get; set; }
    }
}
