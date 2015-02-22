using System.Collections.Generic;
using TimeTable.Domain.OrganizationalStructure;

namespace TimeTable.Domain.Internal
{
    public class UniversityItem
    {
        public int Id { get; set; }
        public University Data { get; set; }
        public List<FacultyItem> Faculties { get; set; }
    }
}