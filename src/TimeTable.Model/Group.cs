using Newtonsoft.Json;

namespace TimeTable.Model
{
    public class Group
    {
        [JsonProperty("group_id")]
        public int Id { get; set; }
        
        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }
}