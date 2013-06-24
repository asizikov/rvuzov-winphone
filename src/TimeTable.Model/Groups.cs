using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Groups
    {
        [JsonProperty("Last")]
        public int LastUpdated { get; set; }
        
        [JsonProperty("groups")]
        public List<Group> GroupsList { get; set; }
    }

    public class Group
    {
        [JsonProperty("group_id")]
        public int GroupId { get; set; }
        
        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }
}