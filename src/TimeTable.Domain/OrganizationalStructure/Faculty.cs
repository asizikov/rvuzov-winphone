using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TimeTable.Domain.OrganizationalStructure
{
    [DebuggerDisplay("Faculty Id = {Id} ,Title = {Title}")]
    public sealed class Faculty
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("startDate")]
        public long StartDate { get; set; }

        [JsonProperty("endDate")]
        public long EndDate { get; set; }

        [JsonProperty("publishSchedule")]
        public bool PublishSchedule { get; set; }

        [JsonProperty("publishFaculty")]
        public bool PublishFaculty { get; set; }
    }

    public sealed class Faculties
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<Faculty> Data { get; set; }
    }
}