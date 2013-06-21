using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTable.Model
{

    public interface IUpdatableModel
    {
        int LastUpdated { get; set; }
    }

    public sealed class UniversitesAll: IUpdatableModel
    {
        [DataMember(Name = "last_updated")]
        public int LastUpdated { get; set; }

        [DataMember(Name = "universities")]
        public List<University> Universities { get; set; }
    }

    public class University
    {
        public int university_id { get; set; }
        public string university_name { get; set; }
        public string university_shortname { get; set; }
        public string parity_countdown { get; set; }
    }

    public class LastUpdated
    {
        public int last_updated { get; set; }
    }
}