using System;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.Phone.Tasks;
using TimeTable.Domain;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Mvvm;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.OrganizationalStructure
{
    public sealed class AuditoriumViewModel : PageViewModel<AuditoriumNavigationParameter>
    {
        private readonly IAsyncDataProvider _dataProvider;
        private readonly IUiStringsProviders _stringProvider;
        private int _id;
        private string _name;

        private string _address;
        private University _university;

        public AuditoriumViewModel([NotNull] IAsyncDataProvider dataProvider,
                                   [NotNull] IUiStringsProviders stringProvider)
        {
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (stringProvider == null) throw new ArgumentNullException("stringProvider");

            _dataProvider = dataProvider;
            _stringProvider = stringProvider;
            ShowInApp = new SimpleCommand(GoToExternalMap);
        }

        public override void Initialize(AuditoriumNavigationParameter navigationParameter)
        {
            ID = navigationParameter.AuditoriumId;
            Init(navigationParameter.UniversityId);
            Name = navigationParameter.AuditoriumName;
            Address = navigationParameter.AuditoriumAddress;
        }

        private void Init(int universityId)
        {
            _dataProvider.GetUniversityByIdAsync(universityId).Subscribe(university =>
            {
                _university = university;
            });
        }

        public ICommand ShowInApp
        {
            get; private set;
        }

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = String.Format(_stringProvider.AuditoryNameTemplate, value);
                OnPropertyChanged("Name");
            }
        }

        private void GoToExternalMap()
        {
            var mapsTask = new BingMapsTask
            {
                SearchTerm = GetSearchQuery(), 
                ZoomLevel = 2
            };

            mapsTask.Show();
        }

        private string GetSearchQuery()
        {
            if (!string.IsNullOrWhiteSpace(Address))
            {
                return Address;
            }
            return _university == null?  string.Empty: _university.Name;
        }
    }
}
