using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public class ShowTeachersTimeTableCommand: ICommand
    {
        private readonly INavigationService _navigationService;

        public ShowTeachersTimeTableCommand([NotNull] INavigationService navigationService)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _navigationService = navigationService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is string)
            {
                var teacherId = parameter as string;
                _navigationService.GoToPage(Pages.Lessons, new List<NavigationParameter>
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = teacherId.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = true.ToString()
                },
            });
            }
            
        }
    }
}
