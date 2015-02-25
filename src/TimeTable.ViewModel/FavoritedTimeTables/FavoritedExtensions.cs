using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Domain.Internal;
using TimeTable.Mvvm.Navigation;

namespace TimeTable.ViewModel.FavoritedTimeTables
{
    internal static class FavoritedExtensions
    {
        [Pure]
        public static List<FavoritedItemViewModel> ToViewModels(this IEnumerable<FavoritedItem> items, INavigationService navigationService)
        {
            return items.Select(i => new FavoritedItemViewModel(i, navigationService)).ToList();
        }
    }
}