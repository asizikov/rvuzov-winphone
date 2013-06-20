using System.Collections.Generic;

namespace TimeTable.Model
{
    public sealed class UniversityAll
    {
        public int last_updated { get; set; }
        public List<University> universities { get; set; }
    }

    public class University
    {
        public int university_id { get; set; }
        public string university_name { get; set; }
        public string university_shortname { get; set; }
        public string parity_countdown { get; set; }
    }
}