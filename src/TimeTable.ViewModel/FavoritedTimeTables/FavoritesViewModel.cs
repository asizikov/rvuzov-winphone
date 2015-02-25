using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Mvvm;
using TimeTable.Mvvm.Navigation;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.FavoritedTimeTables
{
    public sealed class FavoritesViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<FavoritedItemViewModel> _items;

        public FavoritesViewModel([NotNull] INavigationService navigationService,
            [NotNull] FavoritedItemsManager favoritedItemsManager,
            [NotNull] FlurryPublisher flurryPublisher)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (favoritedItemsManager == null) throw new ArgumentNullException("favoritedItemsManager");
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            _navigationService = navigationService;
            flurryPublisher.PublishPageLoadedFavorites();
            Items =
                new ObservableCollection<FavoritedItemViewModel>(
                    favoritedItemsManager.GetFavorites().ToViewModels(_navigationService));
            AddCommand = new SimpleCommand(AddNewFavorite);
        }

        private void AddNewFavorite()
        {
            _navigationService.NavigateTo<UniversitiesPageViewModel, NavigationFlow>(new NavigationFlow
            {
                Reason = Reason.AddingFavorites
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