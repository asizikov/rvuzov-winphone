using TimeTable.Model;

namespace TimeTable.ViewModel.Commands
{
    public interface ICommandFactory
    {
        ITitledCommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher);

        ITitledCommand GetShowGroupTimeTableCommand(University university,LessonGroup group);

        ITitledCommand GetReportErrorCommand();
    }
}
