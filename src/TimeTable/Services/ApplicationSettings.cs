using System;
using System.IO.IsolatedStorage;
using JetBrains.Annotations;
using TimeTable.ViewModel.Enums;
using TimeTable.ViewModel.Services;

namespace TimeTable.Services
{
    public class ApplicationSettings : BaseApplicationSettings
    {
        private void SaveInStorage(string key, object value)
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

        private object LoadFromStorage(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(key)
                       ? IsolatedStorageSettings.ApplicationSettings[key]
                       : null;
        }

        [NotNull] private const string RoleKey = "Role";
        [NotNull] private const string UniversityKey = "University";
        [NotNull] private const string GroupKey = "Group";

        public override UserRole? Role
        {
            get {return (UserRole?) LoadFromStorage(RoleKey); }
            set { SaveInStorage(RoleKey, value); }
        }

        public override int? UniversityId
        {
            get { return (int?) LoadFromStorage(UniversityKey); }
            set { SaveInStorage(UniversityKey, value); }
        }

        public override int? GroupId
        {
            get { return (int?) LoadFromStorage(GroupKey); }
            set { SaveInStorage(GroupKey, value); }
        }
    }
}