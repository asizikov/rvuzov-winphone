using System.Collections.Generic;

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
        public const string IsTeacher = "is_teacher";
        public const string FacultyId = "faculty_id";
    }

    public interface INavigationService
    {
        void GoBack();
        bool CanGoBack();
        void GoToPage(string page, IEnumerable<NavigationParameter> parameters = null);
        void CleanNavigationStack();
    }
}
