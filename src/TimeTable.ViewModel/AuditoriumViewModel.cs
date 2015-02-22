using System;
using System.Windows.Input;
using Microsoft.Phone.Tasks;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class AuditoriumViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly AsyncDataProvider _dataProvider;
        private readonly IUiStringsProviders _stringProvider;
        private int _id;
        private string _name;


        private string _address;
        private University _university;

        public AuditoriumViewModel([NotNull] INavigationService navigation,
                                   [NotNull] AsyncDataProvider dataProvider,
                                   [NotNull] IUiStringsProviders stringProvider)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (stringProvider == null) throw new ArgumentNullException("stringProvider");

            _dataProvider = dataProvider;
            _navigation = navigation;
            _stringProvider = stringProvider;
            ShowInApp = new SimpleCommand(GoToExternalMap);
        }

        public void Initialize(int auditoiumId, int universityId, string auditoriumName, string auditoriumAddress)
        {
            ID = auditoiumId;
            Init(universityId);
            Name = auditoriumName;
            Address = auditoriumAddress;
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
