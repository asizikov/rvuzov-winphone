using System.Collections.Generic;

namespace TimeTable.ViewModel.Services
{

    public class NavigationParameter
    {
        string Parameter { get; set; }
        string Value { get; set; }
    }

    public interface INavigationService
    {
        void GoBack();
        bool CanGoBack();
        void GoToPage(string page, IEnumerable<NavigationParameter> parameters = null);
    }
}
