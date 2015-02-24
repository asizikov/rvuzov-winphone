using System;
using System.Collections.Generic;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;

namespace TimeTable.Domain
{
    public interface IAsyncDataProvider
    {
        void PutUniversity(University university);
        void PutFaculties(int universityId, IEnumerable<Faculty> faculties);
        void PutGroups(int universityId, int facultyId, IEnumerable<Group> groupsList);
        IObservable<Universities> GetUniversitiesAsync();
        IObservable<Groups> GetFacultyGroupsAsync(int facultyId);
        IObservable<Lessons.TimeTable> GetLessonsForGroupAsync(int groupId);
        IObservable<Lessons.TimeTable> GetLessonsForTeacherAsync(int id);
        IObservable<Teachers> GetUniversityTeachersAsync(int universityId);
        IObservable<Faculties> GetUniversityFacultiesAsync(int universityId);
        IObservable<University> GetUniversityByIdAsync(int universityId);
        IObservable<Faculty> GetFacultyByUniversityAndGroupId(int universityId, int groupId);
        IObservable<Teacher> GetTeacherByIdAsync(int universityId, int id);
        IObservable<Group> GetGroupByIdAsync(int facultyId, int id);
        IObservable<Faculty> GetFacultyByIdAsync(int universityId, int facultyId);
    }
}