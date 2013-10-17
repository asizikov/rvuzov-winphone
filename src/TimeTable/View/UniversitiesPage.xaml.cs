using System.Windows;
using System.Windows.Controls;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class UniversitiesPage
    {
        public UniversitiesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = ViewModelLocator.GetUniversitiesViewModel(IsAddingFavorites()) as SearchViewModel;
            DataContext = ViewModel;
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }
    }
}