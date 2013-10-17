using System;
using System.Windows;
using System.Windows.Controls;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class FacultiesPage
    {
        public FacultiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string parameter;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out parameter))
            {
                int universityId;
                if (Int32.TryParse(parameter, out universityId))
                {
                    ViewModel = ViewModelLocator.GetFacultiesPageViewModel(universityId, IsAddingFavorites()) as SearchViewModel;
                    DataContext = ViewModel;
                }
            }
            DataContext = ViewModel;
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }
    }
}