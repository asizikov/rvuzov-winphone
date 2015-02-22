using System.Diagnostics;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel.WeekOverview
{
    [DebuggerDisplay("OptionsItem Title = {Title}")]
    public class OptionsItem
    {
        public string Title { get; set; }
        public ITitledCommand Command { get; set; }
    }
}