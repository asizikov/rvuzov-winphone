using TimeTable.Model.User;

namespace TimeTable.ViewModel.Services
{
    public abstract class BaseApplicationSettings
    {
        public UserRole Role { get; set; }
        public int Group { get; set; }

        public abstract void LoadSettings();
    }
}
