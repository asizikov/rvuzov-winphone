using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class FavoritedItemViewModel : BaseViewModel
    {
        private readonly FavoritedItem _item;
        private readonly IUiStringsProviders _stringsProviders;
        private readonly INavigationService _navigationService;

        public FavoritedItemViewModel([NotNull] FavoritedItem item, [NotNull] IUiStringsProviders stringsProviders,
            [NotNull] INavigationService navigationService)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (stringsProviders == null) throw new ArgumentNullException("stringsProviders");
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            _item = item;
            _stringsProviders = stringsProviders;
            _navigationService = navigationService;
            ShowTimeTable = new SimpleCommand(NavigateToTimetable);
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Title
        {
            get
            {
                return _item.Title.Trim();
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string UniversityName
        {
            get
            {
                return _item.University.Name; //todo: null checks
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand ShowTimeTable { get; private set; }

        private void NavigateToTimetable()
        {
            _navigationService.GoToPage(Pages.Lessons, GetParameters(), 1);
        }

        private IEnumerable<NavigationParameter> GetParameters()
        {
            return new[]
            {
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.Id,
                    Value = _item.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.IsTeacher,
                    Value = (_item.Type == FavoritedItemType.Teacher).ToString()
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.UniversityId,
                    Value = _item.University.Id.ToString(CultureInfo.InvariantCulture)
                },
                new NavigationParameter
                {
                    Parameter = NavigationParameterName.FacultyId,
                    Value =
                        (_item.Type != FavoritedItemType.Teacher
                            ? _item.Faculty.Id.ToString(CultureInfo.InvariantCulture)
                            : "0")
                }
            };
        }
    }
}