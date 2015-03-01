using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Data.Cache;
using TimeTable.Domain;
using TimeTable.Domain.OrganizationalStructure;
using TimeTable.Domain.Participants;
using TimeTable.Networking.Cache;

namespace TimeTable.Data
{
    [UsedImplicitly]
    public class AsyncDataProvider : BaseAsyncWebClient, IAsyncDataProvider
    {
        private readonly Dictionary<int, University> _universities = new Dictionary<int, University>();
        private readonly Dictionary<int, Teacher> _teachers = new Dictionary<int, Teacher>();
        private readonly Dictionary<int, Group> _groups = new Dictionary<int, Group>();
        private readonly Dictionary<int, Faculty> _faculties = new Dictionary<int, Faculty>();

        private readonly RestfulCallFactory _callFactory = new RestfulCallFactory();

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
            GetUniversityFacultiesInternalAsync(university.Id).Subscribe(result =>
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


        public IObservable<Universities> GetUniversitiesAsync()
        {
            return GetUniversitiesInternalAsync();
        }

        private IObservable<Universities> GetUniversitiesInternalAsync(
            CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
        {
            var request = _callFactory.GetAllUniversitesRequest();
            return GetDataAsync(request, cachePolicy);
        }


        public IObservable<Groups> GetFacultyGroupsAsync(int facultyId)
        {
            return GetFacultyGroupsInternalAsync(facultyId);
        }


        private IObservable<Groups> GetFacultyGroupsInternalAsync(int facultyId,
            CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
        {
            var request = _callFactory.GetFacultyGroupsRequest(facultyId);
            return GetDataAsync(request, cachePolicy);
        }

        public IObservable<Domain.Lessons.TimeTable> GetLessonsForGroupAsync(int groupId)
        {
            var groupTimeTableRequest = _callFactory.GetGroupTimeTableRequest(groupId);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<Domain.Lessons.TimeTable> GetLessonsForTeacherAsync(int id)
        {
            var groupTimeTableRequest = _callFactory.GetTeacherTimeTableRequest(id);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<Teachers> GetUniversityTeachersAsync(int universityId)
        {
            var universityTeachersRequest = _callFactory.GetUniversityTeachersRequest(universityId);
            return GetDataAsync(universityTeachersRequest);
        }

        public IObservable<Faculties> GetUniversityFacultiesAsync(int universityId)
        {
            return GetUniversityFacultiesInternalAsync(universityId);
        }

        private IObservable<Faculties> GetUniversityFacultiesInternalAsync(int universityId,
            CachePolicy cachePolicy = CachePolicy.GetFromCacheAndUpdate)
        {
            var request = _callFactory.GetUniversityFacultiesRequest(universityId);
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
                        GetUniversitiesInternalAsync(CachePolicy.TryGetFromCache).Subscribe(universities =>
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

        public Faculty GetFacultyByUniversityAndGroupId(int universityId, int groupId)
        {
            var faculty = _universitiesCache.GetFacultyByGroupAndUniversityIds(universityId, groupId);
            return faculty;
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
                        GetFacultyGroupsInternalAsync(facultyId, CachePolicy.TryGetFromCache).Subscribe(groups =>
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
                        GetUniversityFacultiesInternalAsync(universityId, CachePolicy.TryGetFromCache)
                            .Subscribe(faculties =>
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