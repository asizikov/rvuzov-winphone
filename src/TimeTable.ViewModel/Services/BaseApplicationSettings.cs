using TimeTable.Domain.Internal;

namespace TimeTable.ViewModel.Services
{
    public abstract class BaseApplicationSettings
    {
        public Me Me { get; protected set; }

        public bool IsRegistrationCompleted
        {
            get
            {
                return Me.University != null && Me.Faculty != null && (Me.DefaultGroup != null || Me.Teacher != null);
            }
        }


        public abstract void Save();
    }
}