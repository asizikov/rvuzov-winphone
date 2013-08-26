using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Teachers : IUpdatableModel
    {
        [JsonProperty("teachers")]
        public List<Teacher> TeachersList { get; set; }
        [JsonProperty("last_updated")]
        public int LastUpdated { get; set; }
    }
}
