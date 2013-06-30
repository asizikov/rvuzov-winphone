using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Groups : IUpdatableModel
    {
        [JsonProperty("Last")]
        public int LastUpdated { get; set; }

        [JsonProperty("groups")]
        public List<Group> GroupsList { get; set; }
    }
}