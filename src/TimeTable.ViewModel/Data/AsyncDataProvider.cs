﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Domain;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;

namespace TimeTable.ViewModel.Data
{
    public class AsyncDataProvider : BaseAsyncWebClient
    {
        private readonly Dictionary<int, University> _universities = new Dictionary<int, University>();
        private readonly Dictionary<int, Teacher> _teachers = new Dictionary<int, Teacher>();
        private readonly Dictionary<int, Group> _groups = new Dictionary<int, Group>();
        private readonly Dictionary<int, Faculty> _faculties = new Dictionary<int, Faculty>();

        private readonly UniversitiesCache _universitiesCache;

        public AsyncDataProvider([NotNull] IWebCache cache, [NotNull] UniversitiesCache universitiesCache) : base(cache)
        {
            if (universitiesCache == null) throw new ArgumentNullException("universitiesCache");
            _universitiesCache = universitiesCache;
            _universitiesCache.Load();
        }

        public void PutUniversity(University university)
        {
            _universitiesCache.AddUniversity(university);
            GetUniversitesFacultiesAsync(university.Id).Subscribe(result =>
            {
                if (result != null)
                {
                    PutFaculties(university.Id, result.Data);
                    foreach (var faculty in result.Data)
                    {
                        var id = faculty.Id;
                        GetFacultyGroupsAsync(faculty.Id)
                            .Subscribe(groups =>
                            {
                                if (groups != null)
                                {
                                    PutGroups(university.Id, id, groups.GroupsList);
                                }
                            }, ex => { });
                    }
                }
            }, ex => { });
        }

        public void PutFaculties(int universityId, IEnumerable<Faculty> faculties)
        {
            foreach (var faculty in faculties)
            {
                _universitiesCache.AddFaculty(universityId, faculty);
            }
        }

        public void PutGroups(int universityId, int facultyId, IEnumerable<Group> groupsList)
        {
            foreach (var group in groupsList)
            {
                _universitiesCache.AddGroup(universityId, facultyId, group);
            }
        }

        public IObservable<Universities> GetUniversitesAsync(CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
        {
            var request = CallFactory.GetAllUniversitesRequest();
            return GetDataAsync(request, cachePolicy);
        }

        public IObservable<Groups> GetFacultyGroupsAsync(int facultyId,
            CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
        {
            var request = CallFactory.GetFacultyGroupsRequest(facultyId);
            return GetDataAsync(request, cachePolicy);
        }

        public IObservable<Domain.Lessons.TimeTable> GetLessonsForGroupAsync(int groupId)
        {
            var groupTimeTableRequest = CallFactory.GetGroupTimeTableRequest(groupId);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<Domain.Lessons.TimeTable> GetLessonsForTeacherAsync(int id)
        {
            var groupTimeTableRequest = CallFactory.GetTeacherTimeTableRequest(id);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<Teachers> GetUniversityTeachersAsync(int universityId)
        {
            var universityTeachersRequest = CallFactory.GetUniversityTeachersRequest(universityId);
            return GetDataAsync(universityTeachersRequest);
        }

        public IObservable<Faculties> GetUniversitesFacultiesAsync(int universityId,
            CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
        {
            var request = CallFactory.GetUniversityFacultiesRequest(universityId);
            return GetDataAsync(request, cachePolicy);
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
                        GetUniversitesAsync(CachePolicy.TryGetFromCache).Subscribe(universities =>
                        {
                            var university = universities.Data.FirstOrDefault(u => u.Id == universityId);
                            if (!_universities.ContainsKey(universityId))
                            {
                                _universities.Add(universityId, university);
                            }
                            observer.OnNext(university);
                        });
                    }
                }));
        }

        public IObservable<Faculty> GetFacultyByUniversityAndGroupId(int universityId, int groupId)
        {
            return Observable.Create<Faculty>(observer =>
                Scheduler.Default.Schedule(
                    () =>
                    {
                        var faculty = _universitiesCache.GetFacultyByGroupAndUniversityIds(universityId, groupId);
                        if (faculty != null)
                        {
                            observer.OnNext(faculty);
                        }
                    }));
        }

        public IObservable<Teacher> GetTeacherByIdAsync(int universityId, int id)
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
                            if (!_teachers.ContainsKey(id))
                            {
                                _teachers.Add(id, teacher);
                            }
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
                        GetFacultyGroupsAsync(facultyId, CachePolicy.TryGetFromCache).Subscribe(groups =>
                        {
                            var group = groups.GroupsList.FirstOrDefault(u => u.Id == id);
                            if (!_groups.ContainsKey(id))
                            {
                                _groups.Add(id, group);
                            }
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
                        GetUniversitesFacultiesAsync(universityId, CachePolicy.TryGetFromCache).Subscribe(faculties =>
                        {
                            var faculty = faculties.Data.FirstOrDefault(u => u.Id == facultyId);
                            if (!_faculties.ContainsKey(facultyId))
                            {
                                _faculties.Add(facultyId, faculty);
                            }
                            observer.OnNext(faculty);
                        });
                    }
                }));
        }
    }
}