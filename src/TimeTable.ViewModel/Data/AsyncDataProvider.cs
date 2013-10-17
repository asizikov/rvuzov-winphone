using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel.Data
{
    public class AsyncDataProvider : BaseAsyncDataProvider
    {
        private readonly Dictionary<int, University> _universities = new Dictionary<int, University>();
        private readonly Dictionary<int, Teacher> _teachers = new Dictionary<int, Teacher>();
        private readonly Dictionary<int, Group> _groups = new Dictionary<int, Group>();
        private readonly Dictionary<int, Faculty> _faculties = new Dictionary<int, Faculty>();

        public AsyncDataProvider([NotNull] ICache cache) : base(cache)
        {
        }

        public IObservable<Universities> GetUniversitesAsync()
        {
            var request = CallFactory.GetAllUniversitesRequest();
            return GetDataAsync(request);
        }

        public IObservable<Groups> GetFacultyGroupsAsync(int universityId)
        {
            var request = CallFactory.GetUniversitesGroupsRequest(universityId);
            return GetDataAsync(request);
        }

        public IObservable<Model.TimeTable> GetLessonsForGroupAsync(int groupId)
        {
            var groupTimeTableRequest = CallFactory.GetGroupTimeTableRequest(groupId);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<Model.TimeTable> GetLessonsForTeacherAsync(int id)
        {
            var groupTimeTableRequest = CallFactory.GetTeacherTimeTableRequest(id);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<Teachers> GetUniversityTeachersAsync(int universityId)
        {
            var universityTeachersRequest = CallFactory.GetUniversityTeachersRequest(universityId);
            return GetDataAsync(universityTeachersRequest);
        }

        public IObservable<Faculties> GetUniversitesFacultiesAsync(int universityId)
        {
            var request = CallFactory.GetUniversityFacultiesRequest(universityId);
            return GetDataAsync(request);
        }

        public IObservable<University> GetUniversityByIdAsync(int universityId)
        {
            return Observable.Create<University>(observer =>
                Scheduler.Default.Schedule(() =>
                {
                    if (_universities.ContainsKey(universityId))
                    {
                        observer.OnNext(_universities[universityId]);
                    }
                    else
                    {
                        GetUniversitesAsync().Subscribe(universities =>
                        {
                            var university = universities.Data.FirstOrDefault(u => u.Id == universityId);
                            _universities.Add(universityId, university);
                            observer.OnNext(university);
                        });
                    }
                }));
        }

       public IObservable<Faculty> GetFacultyByUniversityAndGroupId(int universityId, int groupId)
        {
           return Observable.Create<Faculty>(observer =>
                Scheduler.Default.Schedule(() =>
                {
                    GetUniversitesFacultiesAsync(universityId)
                        .Subscribe(f =>
                        {
                            observer.OnNext(new Faculty());
                            //f.Data
                        });
                }));
        } 

        public IObservable<Teacher> GetTeacherByIdAsync(int universityId,int id)
        {
            return Observable.Create<Teacher>(observer =>
                Scheduler.Default.Schedule(() =>
                {
                    if (_teachers.ContainsKey(id))
                    {
                        observer.OnNext(_teachers[id]);
                    }
                    else
                    {
                        GetUniversityTeachersAsync(universityId).Subscribe(teachers =>
                        {
                            var teacher = teachers.TeachersList.FirstOrDefault(u => u.Id == id);
                            _teachers.Add(id, teacher);
                            observer.OnNext(teacher);
                        });
                    }
                }));
        }

        public IObservable<Group> GetGroupByIdAsync(int facultyId, int id)
        {
            return Observable.Create<Group>(observer =>
                Scheduler.Default.Schedule(() =>
                {
                    if (_groups.ContainsKey(id))
                    {
                        observer.OnNext(_groups[id]);
                    }
                    else
                    {
                        GetFacultyGroupsAsync(facultyId).Subscribe(groups =>
                        {
                            var group = groups.GroupsList.FirstOrDefault(u => u.Id == id);
                            _groups.Add(id, group);
                            observer.OnNext(group);
                        });
                    }
                }));
        }

        public IObservable<Faculty> GetFacultyByIdAsync(int universityId, int facultyId)
        {
            return Observable.Create<Faculty>(observer =>
               Scheduler.Default.Schedule(() =>
               {
                   if (_faculties.ContainsKey(facultyId))
                   {
                       observer.OnNext(_faculties[facultyId]);
                   }
                   else
                   {
                       GetUniversitesFacultiesAsync(universityId).Subscribe(faculties =>
                       {
                           var faculty = faculties.Data.FirstOrDefault(u => u.Id == facultyId);
                           _faculties.Add(facultyId, faculty);
                           observer.OnNext(faculty);
                       });
                   }
               }));
        }
    }
}