namespace TimeTable.ViewModel.OrganizationalStructure
{
    public class NavigationFlow
    {
        public Reason Reason { get; set; }
        public int UniversityId { get; set; }
        public int FacultyId { get; set; }
        public bool IsTeacher { get; set; }
        public string UniversityName { get; set; }
        public string FacultyName { get; set; }
    }
}