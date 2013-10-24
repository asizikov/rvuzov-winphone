using System.ComponentModel;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public abstract class SearchPage : PhoneApplicationPage
    {
        protected SearchViewModel ViewModel;
        protected const string SEARCH_KEY = "search_visibility_key";

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

        protected abstract void SaveState(NavigatingCancelEventArgs e);

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            State.Clear();
            if (this.ShouldTombstone(e))
            {
                SaveState(e);
            }
        }
    }
}