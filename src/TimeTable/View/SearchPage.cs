using System.ComponentModel;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public class SearchPage : PhoneApplicationPage
    {
        protected SearchViewModel ViewModel;

        private bool IsAddingFavorites()
        {
            return NavigationContext.QueryString.ContainsKey(NavigationParameterName.AddFavorites);
        }

        protected Reason GetReason()
        {
            if(IsAddingFavorites()) return Reason.AddingFavorites;
            if (IsChangingDefault()) return Reason.ChangeDefault;
            return Reason.Registration;
        }

        private bool IsChangingDefault()
        {
            return NavigationContext.QueryString.ContainsKey(NavigationParameterName.ChangeDefault);
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