using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Domain.Participants
{
    public sealed class Groups
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Group> GroupsList { get; set; }
    }
}