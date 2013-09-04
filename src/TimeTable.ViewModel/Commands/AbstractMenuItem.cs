using System.Windows.Input;

namespace TimeTable.ViewModel.Commands
{
    public class AbstractMenuItem
    {
        public object CommandParameter { get; set; }
        public ICommand Command { get; set; }
        public string Header { get; set; }
    }
}
