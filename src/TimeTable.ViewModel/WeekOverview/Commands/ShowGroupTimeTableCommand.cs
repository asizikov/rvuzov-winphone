using System;
using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Domain.Lessons;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.WeekOverview.Commands
{
    public sealed class ShowGroupTimeTableCommand : ITitledCommand
    {
        private readonly INavigationService _navigationService;
        private readonly FlurryPublisher _flurryPublisher;
        private readonly IAsyncDataProvider _dataProvider;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly University _university;
        private readonly LessonGroup _group;


        public ShowGroupTimeTableCommand([NotNull] INavigationService navigationService,
            [NotNull] FlurryPublisher flurryPublisher, [NotNull] IAsyncDataProvider dataProvider,
            [NotNull] IUiStringsProviders stringsProviders, [NotNull] University university, [NotNull] LessonGroup group)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            if (dataProvider == null) throw new ArgumentNullException("dataProvider");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            if (university == null) throw new ArgumentNullException("university");
            if (group == null) throw new ArgumentNullException("group");
            _navigationService = navigationService;
            _flurryPublisher = flurryPublisher;
            _dataProvider = dataProvider;
            _stringsProviders = stringsProviders;
            _university = university;
            _group = group;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _dataProvider.GetFacultyByUniversityAndGroupId(_university.Id, _group.Id)
                .Subscribe(faculty =>
                {
                    _flurryPublisher.PublishContextMenuShowGroupTimeTable(_university, _group.GroupName, _group.Id);
                    var navigationParameter = new LessonsNavigationParameter
                    {
                        Id = _group.Id,
                        IsTeacher = false,
                        FacultyId = faculty.Id,
                        UniversityId = _university.Id
                    };
                    _navigationService.NavigateTo<LessonsPageViewModel, LessonsNavigationParameter>(navigationParameter);
                });
        }

        public string Title
        {
            get { return _stringsProviders.GroupTimeTable; }
        }
    }
}