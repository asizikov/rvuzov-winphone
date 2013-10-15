using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class DefaultParams
    {
        [JsonProperty("defaultgroup")]
        public string DefaultGroup { get; set; }

        [JsonProperty("defaultuniversity")]
        public string DefaultUniversity { get; set; }
    }
}
