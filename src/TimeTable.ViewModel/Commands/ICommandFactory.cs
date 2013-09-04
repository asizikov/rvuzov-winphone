using System.Windows.Input;

namespace TimeTable.ViewModel.Commands
{
    public interface ICommandFactory
    {
        ICommand GetShowTeachersTimeTableCommand();
    }
}
