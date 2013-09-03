using System.Collections.Generic;
using System.Linq;
using TimeTable.Model;

namespace TimeTable.ViewModel.Extensions
{
    public static class DayExtensions
    {
        public static IEnumerable<DayViewModel> ToViewModelList(this IEnumerable<Day> models, WeekType type)
        {
            var list = models.Select(day => new DayViewModel(day, type));
            return list;
        }
    }
}