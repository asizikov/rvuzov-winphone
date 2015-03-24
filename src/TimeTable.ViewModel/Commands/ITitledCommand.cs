using System.Windows.Input;

namespace TimeTable.ViewModel.Commands
{
    public interface ITitledCommand : ICommand
    {
        string Title { get; }
    }
}