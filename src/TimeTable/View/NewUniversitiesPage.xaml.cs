using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof (UniversitiesPageViewModel))]
    public partial class NewUniversitiesPage : PhoneApplicationPage
    {
        public NewUniversitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navigationContext = NavigationContext.QueryString.RestoreContext<Reason>();
            DataContext = ViewModelLocator.GetUniversitiesViewModel(navigationContext.Body) as ISearchViewModel;
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }
    }
}