using System.Collections.Generic;

namespace TimeTable.Model.Internal
{
    public class UniversityItem
    {
        public int Id { get; set; }
        public University Data { get; set; }
        public List<FacultyItem> Faculties { get; set; }
    }
}