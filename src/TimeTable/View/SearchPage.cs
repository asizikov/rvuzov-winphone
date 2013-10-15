using System.ComponentModel;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public class SearchPage : PhoneApplicationPage
    {
        protected SearchViewModel ViewModel;

        protected bool IsAddingFavorites()
        {
            return NavigationContext.QueryString.ContainsKey(NavigationParameterName.AddFavorites);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (ViewModel != null && ViewModel.IsSearchBoxVisible)
            {
                ViewModel.ResetSearchState();
                e.Cancel = true;
            }
        }
    }
}