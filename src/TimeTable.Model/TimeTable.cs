﻿using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class TimeTable
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public TimeTableData Data { get; set; }
    }
}