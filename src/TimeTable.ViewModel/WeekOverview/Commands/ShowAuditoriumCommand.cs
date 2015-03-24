using System;
using JetBrains.Annotations;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.WeekOverview.Commands
{
    public sealed class ShowAuditoriumCommand : ITitledCommand
    {
        private readonly INavigationService _navigationService;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly int _universityId;
        private readonly Auditorium _auditorium;

        public ShowAuditoriumCommand([NotNull] INavigationService navigationService,
                                     [NotNull] IUiStringsProviders stringsProviders, int universityId,
                                     [NotNull] Auditorium auditorium)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _navigationService = navigationService;
            _stringsProviders = stringsProviders;
            _universityId = universityId;
            _auditorium = auditorium;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var address = String.IsNullOrEmpty(_auditorium.Address) ? string.Empty : _auditorium.Address;
            var name = String.IsNullOrEmpty(_auditorium.Name) ? string.Empty : _auditorium.Name;

            _navigationService.NavigateTo<AuditoriumViewModel, AuditoriumNavigationParameter>(
                new AuditoriumNavigationParameter
                {
                    AuditoriumId = _auditorium.Id,
                    AuditoriumName = name,
                    AuditoriumAddress = address,
                    UniversityId = _universityId
                });
        }

        public event EventHandler CanExecuteChanged;

        public string Title
        {
            get { return _stringsProviders.Auditory; }
        }
    }
}