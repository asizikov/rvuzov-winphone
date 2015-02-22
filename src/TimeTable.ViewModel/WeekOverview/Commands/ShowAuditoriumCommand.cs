using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.ViewModel.Commands;
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
            [NotNull] IUiStringsProviders stringsProviders, int universityId, [NotNull] Auditorium auditorium)
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
            var address = String.IsNullOrEmpty(_auditorium.Address) ? String.Empty : _auditorium.Address;
            var name = String.IsNullOrEmpty(_auditorium.Name) ? String.Empty : _auditorium.Name;

            _navigationService.GoToPage(Pages.Auditoriums, new List<NavigationParameter>
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = _auditorium.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Name,
                    Value = name
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Address,
                    Value = address
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.UniversityId,
                    Value = _universityId.ToString(CultureInfo.InvariantCulture)
                }
            });
        }

        public event EventHandler CanExecuteChanged;

        public string Title
        {
            get { return _stringsProviders.Auditory; }
        }
    }
}