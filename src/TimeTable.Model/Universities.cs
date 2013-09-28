using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTable.Model
{
    public sealed class Universities//: IUpdatableModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<University> Data { get; set; }

        //[JsonProperty("Last")]
        //public int LastUpdated { get; set; }

        //[JsonProperty("universities")]
        //public List<University> UniversitesList { get; set; }
    }

    public sealed class University
    {
        /*        "email": null,
                "title": "Алтайский государственный университет",
                "shortTitle": "АлтГУ",
              "allFacultiesTimestamp": null,
                "allGroupsTimestamp": null,
                "allTeachersTimestamp": null,
                "commonScheduleTimestamp": null,
                "startDate": "1378080000",
                "endDate": "1391040000",
                "publishSchedule": true,
                "publishUniversity": true
         */

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