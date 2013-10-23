using System;
using JetBrains.Annotations;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class AuditoriumViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;
        private readonly AsyncDataProvider _dataProvider;
        private readonly IUiStringsProviders _stringProvider;

        #region ID
        private int _id;

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        #endregion

        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = String.Format(_stringProvider.AuditoryNameTemplate, value);
                OnPropertyChanged("Name");
            }
        }
        #endregion

        #region Address
        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }
        #endregion

        public AuditoriumViewModel([NotNull] INavigationService navigation,
                                   [NotNull] AsyncDataProvider dataProvider,
                                   [NotNull] IUiStringsProviders stringProvider,
                                   int auditoiumID,
                                   string auditoriumName,
                                   string auditoriumAddress)
        {
            if (navigation == null) throw new ArgumentNullException("navigation");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (stringProvider == null) throw new ArgumentNullException("stringProvider");


            _dataProvider = dataProvider;
            _navigation = navigation;
            _stringProvider = stringProvider;

            ID = auditoiumID;

            //TODO нужно выпилить сохранение имени и адреса, ибо все через АПИ
            Name = auditoriumName;
            Address = auditoriumAddress;
        }
    }
}
