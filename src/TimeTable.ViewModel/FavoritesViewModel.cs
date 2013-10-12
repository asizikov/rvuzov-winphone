using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Extensions;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public sealed class FavoritesViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly BaseApplicationSettings _applicationSettings;
        private readonly FavoritedItemsManager _favoritedItemsManager;
        private ObservableCollection<FavoritedItemViewModel> _items;

        public FavoritesViewModel([NotNull] INavigationService navigationService,
            [NotNull] BaseApplicationSettings applicationSettings, [NotNull] FavoritedItemsManager favoritedItemsManager)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (applicationSettings == null) throw new ArgumentNullException("applicationSettings");
            if (favoritedItemsManager == null) throw new ArgumentNullException("favoritedItemsManager");
            _navigationService = navigationService;
            _applicationSettings = applicationSettings;
            _favoritedItemsManager = favoritedItemsManager;
            Items = new ObservableCollection<FavoritedItemViewModel>(_favoritedItemsManager.GetFavorites().ToViewModels());
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
    }

    public sealed class FavoritedItemViewModel : BaseViewModel
    {
        private readonly FavoritedItem _item;

        public FavoritedItemViewModel([NotNull] FavoritedItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            _item = item;
        }

        public string Title
        {
            get
            {
                return _item.Title;
            }
        }
    }
}