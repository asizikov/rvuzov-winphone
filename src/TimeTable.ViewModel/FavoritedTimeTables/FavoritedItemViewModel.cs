using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.Mvvm;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.WeekOverview;

namespace TimeTable.ViewModel.FavoritedTimeTables
{
    public sealed class FavoritedItemViewModel : BaseViewModel
    {
        private readonly FavoritedItem _item;
        private readonly INavigationService _navigationService;

        public FavoritedItemViewModel([NotNull] FavoritedItem item, [NotNull] INavigationService navigationService)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _item = item;
            _navigationService = navigationService;
            ShowTimeTable = new SimpleCommand(NavigateToTimetable);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Title
        {
            get { return _item.Title.Trim(); }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string UniversityName
        {
            get { return _item.University.Name; //todo: null checks
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand ShowTimeTable { get; private set; }

        private void NavigateToTimetable()
        {
            _navigationService.NavigateTo<LessonsPageViewModel, LessonsNavigationParameter>(GetParameters(), 1);
        }

        private LessonsNavigationParameter GetParameters()
        {
            return new LessonsNavigationParameter
            {
                Id = _item.Id,
                IsTeacher = _item.Type == FavoritedItemType.Teacher,
                FacultyId = _item.Type != FavoritedItemType.Teacher
                    ? _item.Faculty.Id
                    : 0,
                UniversityId = _item.University.Id
            };
        }
    }
}