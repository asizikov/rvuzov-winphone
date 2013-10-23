using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public sealed class ShowAuditoriumCommand : ITitledCommand
    {
        private readonly INavigationService _navigationService;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly Auditorium _auditorium;

        //TODO нужен ли flurryPublisher и для чего
        public ShowAuditoriumCommand([NotNull] INavigationService navigationService,
                                     [NotNull] IUiStringsProviders stringsProviders,
                                     [NotNull] Auditorium auditorium)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            _navigationService = navigationService;
            _stringsProviders = stringsProviders;
            _auditorium = auditorium;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //TODO подпорки, которые уйдут при получении данных через АПИ
            var address = String.IsNullOrEmpty(_auditorium.Address) ? String.Empty : _auditorium.Address;
            var name = String.IsNullOrEmpty(_auditorium.Name) ? String.Empty : _auditorium.Name;

            //TODO Достаточно передать только auditoiumID, остальное запросами, но в АПИ пока не разобрался до конца
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
