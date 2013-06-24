using System;
using Microsoft.Phone.Controls;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TinyIoC;

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
                    new GroupPageViewModel(new AsyncDataProvider(), TinyIoCContainer.Current.Resolve<INavigationService>(), id);
                }
            }
        }
    }
}