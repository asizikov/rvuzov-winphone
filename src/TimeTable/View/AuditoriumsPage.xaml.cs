using System;
using System.Windows.Navigation;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class AuditoriumsPage
    {
        public AuditoriumsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string auditoriumIDString;
            string name;
            string address;
            string universityIdString;

            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out auditoriumIDString) &&
                NavigationContext.QueryString.TryGetValue(NavigationParameterName.Name, out name) &&
                NavigationContext.QueryString.TryGetValue(NavigationParameterName.UniversityId, out universityIdString) &&
                NavigationContext.QueryString.TryGetValue(NavigationParameterName.Address, out address))
            {
                int auditoiumId;
                int universityId;
                if (Int32.TryParse(auditoriumIDString, out auditoiumId) &&
                    Int32.TryParse(universityIdString, out universityId))
                {
                    DataContext = ViewModelLocator.GetAuditoriumViewModel(auditoiumId, universityId ,name, address);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}