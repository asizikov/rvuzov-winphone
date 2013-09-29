using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class Universities
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<University> Data { get; set; }
    }

    public sealed class University
    {
        [CanBeNull, JsonProperty("email")]
        public int? Email { get; set; }

        [CanBeNull, JsonProperty("allFacultiesTimestamp")]
        public long? AllFacultiesTimestamp { get; set; }

        [CanBeNull, JsonProperty("allGroupsTimestamp")]
        public long? AllGroupsTimestamp { get; set; }

        [CanBeNull, JsonProperty("allTeachersTimestamp")]
        public long? AllTeachersTimestamp { get; set; }

        [CanBeNull, JsonProperty("commonScheduleTimestamp")]
        public long? CommonScheduleTimestamp { get; set; }

        [JsonProperty("startDate")]
        public long StartDate { get; set; }

        [JsonProperty("endDate")]
        public long EndDate { get; set; }

        [JsonProperty("publishSchedule")]
        public bool PublishSchedule { get; set; }

        [JsonProperty("publishUniversity")]
        public bool PublishUniversity { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("shortTitle")]
        public string ShortName { get; set; }

        [JsonProperty("parity_countdown")]
        public string ParityCountdown { get; set; }
    }
}