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

        public IObservable<Groups> GetUniversitesGroupsAsync(int universityId)
        {
            var request = CallFactory.GetUniversitesGroupsRequest(universityId);
            return GetDataAsync(request);
        }

        public IObservable<GroupTimeTable> GetLessonsForGroupAsync(int groupId)
        {
            var groupTimeTableRequest = CallFactory.GetGroupTimeTableRequest(groupId);
            return GetDataAsync(groupTimeTableRequest);
        }

        public IObservable<University> GetUniversityByIdAsync(int universityId)
        {
            return Observable.Create<University>(observer =>
                Scheduler.Default.Schedule(() =>
                    GetUniversitesAsync().Subscribe(universities =>
                    {
                        var university = universities.UniversitesList.FirstOrDefault(u => u.Id == universityId);
                        observer.OnNext(university);
                    })));
        }

        public IObservable<Teachers> GetUniversityTeachersAsync(int universityId)
        {
            var universityTeachersRequest = CallFactory.GetUniversityTeachersRequest(universityId);
            return GetDataAsync(universityTeachersRequest);
        }
    }
}