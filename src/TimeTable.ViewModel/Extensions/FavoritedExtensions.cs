using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.Extensions
{
    internal static class FavoritedExtensions
    {
        [Pure]
        public static List<FavoritedItemViewModel> ToViewModels(this IEnumerable<FavoritedItem> items, IUiStringsProviders stringsProviders, INavigationService navigationService)
        {
            return items.Select(i => new FavoritedItemViewModel(i, stringsProviders, navigationService)).ToList();
        }
    }
}