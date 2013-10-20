using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JetBrains.Annotations;
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
}