﻿using Newtonsoft.Json;

namespace TimeTable.Model
{
    public enum FavoritedItemType
    {
        Unknown = 0,
        Group = 1,
        Teacher = 2
    }

    public sealed class FavoritedItem
    {
        [JsonProperty("type")]
        public FavoritedItemType Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("faculty")]
        public Faculty Faculty { get; set; }

        [JsonProperty("university")]
        public University University { get; set; }
    }
}