using System.Collections.Generic;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;

namespace TimeTable.Domain.Internal
{
    public class FacultyItem
    {
        public int Id { get; set; }
        public Faculty Data { get; set; }
        public List<Group> Groups { get; set; }
    }
}