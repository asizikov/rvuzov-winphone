using TimeTable.ViewModel.Enums;

namespace TimeTable.ViewModel.Services
{
    public abstract class BaseApplicationSettings
    {
        public abstract UserRole? Role { get; set; }
        public abstract int? UniversityId { get; set; }
        public abstract int? GroupId { get; set; }
        public abstract int? FacultyId { get; set; }

        public abstract string GroupName { get; set; }

        public bool FirstLoad { get; set; }

        public bool IsRegistrationCompleted
        {
            get
            {
                return UniversityId != null && GroupId != null && FacultyId != null;
            }
        }

        protected BaseApplicationSettings()
        {
            FirstLoad = true;
        }
    }
}