using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using TimeTable.ViewModel.Enums;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public class ApplicationSettings : BaseApplicationSettings
    {
        private static void SaveToStorage(string key, object value)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        [CanBeNull]
        private static object LoadFromStorage(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(key)
                ? IsolatedStorageSettings.ApplicationSettings[key]
                : null;
        }

        [NotNull] private const string ROLE_KEY = "Role";
        [NotNull] private const string UNIVERSITY_KEY = "University";
        [NotNull] private const string GROUP_KEY = "Group";
        [NotNull] private const string GROUP_NAME_KEY = "GroupName";
        [NotNull] private const string FACULTY_KEY = "FacultyId";

        public override UserRole? Role
        {
            get { return (UserRole?) LoadFromStorage(ROLE_KEY); }
            set { SaveToStorage(ROLE_KEY, value); }
        }

        public override int? UniversityId
        {
            get { return (int?) LoadFromStorage(UNIVERSITY_KEY); }
            set { SaveToStorage(UNIVERSITY_KEY, value); }
        }

        public override int? GroupId
        {
            get { return (int?) LoadFromStorage(GROUP_KEY); }
            set { SaveToStorage(GROUP_KEY, value); }
        }

        public override string GroupName
        {
            get { return (string) LoadFromStorage(GROUP_NAME_KEY); }
            set { SaveToStorage(GROUP_NAME_KEY, value); }
        }

        public override int? FacultyId
        {
            get { return (int?) LoadFromStorage(FACULTY_KEY); }
            set
            {
                SaveToStorage(FACULTY_KEY, value);
            }

        }
    }
}