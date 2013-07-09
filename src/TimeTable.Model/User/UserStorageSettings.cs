using System.IO.IsolatedStorage;
using JetBrains.Annotations;

namespace TimeTable.Model.User
{
    public static class UserStorageSettings
    {
        [NotNull] private const string LastPage = "LastPage";


        public static void SetLastPage(string pageName)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(UserStorageSettings.LastPage))
            {
                IsolatedStorageSettings.ApplicationSettings[LastPage] = pageName;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add(LastPage, pageName);
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static string GetLastPage()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(LastPage))
                return null;

            return (string) IsolatedStorageSettings.ApplicationSettings[LastPage];
        }
    }
}