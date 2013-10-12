using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel.Extensions
{
    internal static class FavoritedExtensions
    {
        [Pure]
        public static List<FavoritedItemViewModel> ToViewModels(this IEnumerable<FavoritedItem> items)
        {
            return items.Select(i => new FavoritedItemViewModel(i)).ToList();
        }
    }
}