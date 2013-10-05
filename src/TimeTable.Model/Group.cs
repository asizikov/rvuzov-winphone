using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class Group
    {
        [JsonProperty("group_id")]
        public int Id { get; set; }

        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        [CanBeNull,JsonProperty("speciality")]
        public string Speciality { get; set; }

        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }
    }
}