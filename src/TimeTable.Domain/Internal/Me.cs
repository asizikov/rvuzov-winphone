using Newtonsoft.Json;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;

namespace TimeTable.Domain.Internal
{
    public sealed class Me
    {
        [JsonProperty("group")]
        public Group DefaultGroup { get; set; }

        [JsonProperty("university")]
        public University University { get; set; }

        [JsonProperty("faculty")]
        public Faculty Faculty { get; set; }

        [JsonProperty("role")]
        public UserRole Role { get; set; }

        [JsonProperty("teacher")]
        public Teacher Teacher { get; set; }

        [JsonProperty("tmp_university")]
        public University TemporaryUniversity { get; set; }

        [JsonProperty("tmp_faculty")]
        public Faculty TemporaryFaculty { get; set; }
    }
}
