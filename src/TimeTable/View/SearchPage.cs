using System.ComponentModel;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel;

namespace TimeTable.View
{
    public class SearchPage : PhoneApplicationPage
    {
        protected SearchViewModel ViewModel;

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