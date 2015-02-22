﻿using TimeTable.Domain;
using TimeTable.Domain.Lessons;
using TimeTable.Domain.OrganizationalStructure;

namespace TimeTable.ViewModel.Commands
{
    public interface ICommandFactory
    {
        ITitledCommand GetShowTeachersTimeTableCommand(University university, LessonTeacher teacher);
        ITitledCommand GetShowGroupTimeTableCommand(University university, LessonGroup group);
        ITitledCommand GetShowAuditoriumCommand(Auditorium auditorium, int universityId);
        ITitledCommand GetReportErrorCommand(int holderId,int lessonId, bool isTeacher);
        ITitledCommand GetUpdateLessonCommand();
    }
}
