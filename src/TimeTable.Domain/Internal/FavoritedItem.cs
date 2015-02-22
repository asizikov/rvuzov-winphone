using System.Collections.Generic;
using Newtonsoft.Json;
using TimeTable.Domain.OrganizationalStructure;

namespace TimeTable.Domain.Internal
{
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

    public sealed class Favorites
    {
        [JsonProperty("favorites")]
        public List<FavoritedItem> Items { get; set; } 
    }
}