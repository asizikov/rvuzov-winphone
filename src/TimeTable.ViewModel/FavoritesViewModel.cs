using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Extensions;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class FavoritesViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly FavoritedItemsManager _favoritedItemsManager;
        private ObservableCollection<FavoritedItemViewModel> _items;

        public FavoritesViewModel([NotNull] INavigationService navigationService,
            [NotNull] FavoritedItemsManager favoritedItemsManager, IUiStringsProviders stringsProviders)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (favoritedItemsManager == null) throw new ArgumentNullException("favoritedItemsManager");
            _navigationService = navigationService;
            _favoritedItemsManager = favoritedItemsManager;
            Items =
                new ObservableCollection<FavoritedItemViewModel>(
                    _favoritedItemsManager.GetFavorites().ToViewModels(stringsProviders, _navigationService));
            AddCommand = new SimpleCommand(AddNewFavorite);
        }

        private void AddNewFavorite()
        {
            _navigationService.GoToPage(Pages.Universities, new[]
            {
                new NavigationParameter
                {   
                    Parameter = NavigationParameterName.AddFavorites,
                    Value = true.ToString()
                }
            });
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ObservableCollection<FavoritedItemViewModel> Items
        {
            get { return _items; }
            private set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand AddCommand { get; private set; }
    }

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
                return _item.Type == FavoritedItemType.Group
                    ? string.Format("{0}: {1}", _stringsProviders.Group, _item.Title)
                    : _item.Title;
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