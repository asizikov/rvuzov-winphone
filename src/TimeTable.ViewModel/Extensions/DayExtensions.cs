using System.Collections.Generic;
using System.Linq;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel.Extensions
{
    public static class DayExtensions
    {
        public static IEnumerable<DayViewModel> ToViewModelList(this IEnumerable<Day> models,
            ICommandFactory commandFactory, WeekType type, University university)
        {
            var list = models.Select(day => new DayViewModel(day, type, commandFactory, university));
            return list;
        }
    }
}