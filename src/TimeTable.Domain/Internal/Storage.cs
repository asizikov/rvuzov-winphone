using System.Collections.Generic;

namespace TimeTable.Domain.Internal
{
    public class Storage
    {
        public int Version { get; set; }
        public List<UniversityItem> Data { get; set; }
    }
}