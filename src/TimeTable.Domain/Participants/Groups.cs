using System.Collections.Generic;
using Newtonsoft.Json;
using TimeTable.Domain.People;

namespace TimeTable.Domain.OrganizationalStructure
{
    public sealed class Groups
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Group> GroupsList { get; set; }
    }
}