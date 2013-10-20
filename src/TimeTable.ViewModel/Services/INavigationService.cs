using System.Collections.Generic;
using JetBrains.Annotations;

namespace TimeTable.ViewModel.Services
{

    public sealed class NavigationParameter
    {
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public static class NavigationParameterName
    {
        public const string UniversityId = "university_id";
        public const string Id = "id";
        public const string Name = "name";
        public const string Address = "address";
        public const string IsTeacher = "is_teacher";
        public const string FacultyId = "faculty_id";
        public const string AddFavorites = "add_favorites";
    }

    public interface INavigationService
    {
        void GoBack();
        bool CanGoBack();
        void GoToPage(string page, IEnumerable<NavigationParameter> parameters = null);
        void CleanNavigationStack();
        void GoToPage(string page, [CanBeNull] IEnumerable<NavigationParameter> parameters, int numberOfItemsToRemove);
    }
}
