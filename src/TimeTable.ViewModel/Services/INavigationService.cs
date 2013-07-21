using System.Collections.Generic;

namespace TimeTable.ViewModel.Services
{

    public class NavigationParameter
    {
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public static class NavigationParameterName
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public interface INavigationService
    {
        void GoBack();
        bool CanGoBack();
        void GoToPage(string page, IEnumerable<NavigationParameter> parameters = null);
    }
}
