using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Commands
{
    public sealed class ShowTeachersTimeTableCommand : ITitledCommand
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly University _university;
        private readonly LessonTeacher _teacher;


        public ShowTeachersTimeTableCommand([NotNull] INavigationService navigationService, [NotNull] FlurryPublisher flurryPublisher,
            [NotNull] IUiStringsProviders stringsProviders, [NotNull] University university, [NotNull] LessonTeacher teacher)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            if (university == null) throw new ArgumentNullException("university");
            if (teacher == null) throw new ArgumentNullException("teacher");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _stringsProviders = stringsProviders;
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

        public string Title { get { return _stringsProviders.TeachersTimeTable; } }
    }
}