using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Model;

namespace TimeTable.ViewModel.Data
{
    public class AsyncDataProvider : BaseAsyncDataProvider
    {
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

        public IObservable<University> GetUniversityByIdAsync(int universityId)
        {
            return Observable.Create<University>(observer =>
                Scheduler.Default.Schedule(() =>
                    GetUniversitesAsync().Subscribe(universities =>
                    {
                        var university = universities.Data.FirstOrDefault(u => u.Id == universityId);
                        observer.OnNext(university);
                    })));
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
    }
}