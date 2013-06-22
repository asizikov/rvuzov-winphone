using System;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class GroupsPage : PhoneApplicationPage
    {
        public GroupsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string parameter;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out parameter))
            {
                int id;
                if (Int32.TryParse(parameter, out id))
                {
                }
            }
        }
    }
}