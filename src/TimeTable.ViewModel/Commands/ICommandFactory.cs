using TimeTable.Model;

namespace TimeTable.ViewModel.Commands
{
    public interface ICommandFactory
    {
        ITitledCommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher);
    }
}
