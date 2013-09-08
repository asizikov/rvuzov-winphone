using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public class ShowTeachersTimeTableCommand : ICommand
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly University _university;
        private readonly Teacher _teacher;


        public ShowTeachersTimeTableCommand([NotNull] INavigationService navigationService,
            [NotNull] FlurryPublisher flurryPublisher, [NotNull] University university, [NotNull] Teacher teacher)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (university == null) throw new ArgumentNullException("university");
            if (teacher == null) throw new ArgumentNullException("teacher");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _university = university;
            _teacher = teacher;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _flurryPublisher.PublishContextMenuShowTeachersTimeTable(_university, _teacher.Name, _teacher.Id);
            _navigationService.GoToPage(Pages.Lessons, new List<NavigationParameter>
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = _teacher.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = true.ToString()
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.UniversityId,
                    Value = _university.Id.ToString(CultureInfo.InvariantCulture)
                }
            });
        }
    }
}