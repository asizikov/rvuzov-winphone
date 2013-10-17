using Newtonsoft.Json;

namespace TimeTable.Model
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
    }
}
