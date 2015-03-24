using TimeTable.Domain.Lessons;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.ViewModel.Commands
{
    public interface ICommandFactory
    {
        ITitledCommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher);
        ITitledCommand GetShowGroupTimeTableCommand(University university, LessonGroup group);
        ITitledCommand GetShowAuditoriumCommand(Auditorium auditorium, int universityId);
        ITitledCommand GetUpdateLessonCommand(NavigationFlow navigationFlow, Group group);
    }
}