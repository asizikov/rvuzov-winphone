using System.Windows.Input;
using TimeTable.Model;

namespace TimeTable.ViewModel.Commands
{
    public interface ICommandFactory
    {
        ICommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher);
    }
}
