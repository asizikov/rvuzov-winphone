using TimeTable.ViewModel.Enums;

namespace TimeTable.ViewModel.Services
{
    public abstract class BaseApplicationSettings
    {
        public abstract UserRole? Role { get; set; }
        public abstract int? UniversityId { get; set; }
        public abstract int? GroupId { get; set; }
        public abstract string GroupName { get; set; }
        public abstract int? FacultyId { get; set; }

        public bool FirstLoad { get; set; }

        protected BaseApplicationSettings()
        {
            FirstLoad = true;
        }
    }
}
