using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Domain.OrganizationalStructure
{
    public sealed class Universities
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<University> Data { get; set; }
    }
}