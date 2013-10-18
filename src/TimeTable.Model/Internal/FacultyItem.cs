using System.Collections.Generic;

namespace TimeTable.Model.Internal
{
    public class FacultyItem
    {
        public int Id { get; set; }
        public Faculty Data { get; set; }
        public List<Group> Groups { get; set; }
    }
}
