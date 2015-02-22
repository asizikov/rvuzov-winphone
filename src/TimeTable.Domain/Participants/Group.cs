using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Domain.Participants
{
    public sealed class Group
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string GroupName { get; set; }

        [CanBeNull,JsonProperty("speciality")]
        public string Speciality { get; set; }

        [JsonProperty("timestamp")]
        public long? TimeStamp { get; set; }
    }
}