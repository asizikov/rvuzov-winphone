using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking.Restful;
using TimeTable.ViewModel.Restful;

namespace TimeTable.ViewModel.Data
{
    public class AsyncDataProvider
    {
        private readonly RestfulCallFactory _callFactory = new RestfulCallFactory();
        private readonly ICache _cache;

        public AsyncDataProvider([NotNull] ICache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");

            _cache = cache;
        }

        public IObservable<Universities> GetUniversitesAsync()
        {
            var request = _callFactory.GetAllUniversitesRequest();
            return GetDataAsync(request);
        }

        public IObservable<Groups> GetUniversitesGroupsAsync(int universityId)
        {
            var request = _callFactory.GetUniversitesGroupsRequest(universityId);
            return GetDataAsync(request);
        }

        public IObservable<GroupTimeTable> GetLessonsForGroupAsync(int groupId)
        {
            var groupTimeTableRequest = _callFactory.GetGroupTimeTableRequest(groupId);
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
            var universityTeachersRequest = _callFactory.GetUniversityTeachersRequest(universityId);
            return GetDataAsync(universityTeachersRequest);
        }

        private IObservable<T> GetDataAsync<T>(RestfullRequest<T> request) where T : new()
        {
            if (!_cache.IsCached<T>(request.Url))
            {
                return Observable.Create<T>(
                    observer =>
                        Scheduler.Default.Schedule(() => ExecuteRequest(request, observer))
                    );
            }
            return Observable.Create<T>(
                observer =>
                    Scheduler.Default.Schedule(() =>
                    {
                        var item = _cache.Fetch<T>(request.Url);
                        observer.OnNext(item);

                        var updatable = item as IUpdatableModel;
                        if (updatable != null)
                        {
                            var lastUpdated = updatable.LastUpdated;
                            var lastUpdatedRequest = _callFactory.GetLastUpdatedRequest<T>();
                            lastUpdatedRequest.Execute()
                                .Subscribe(
                                    resutl =>
                                    {
                                        if (resutl.Last > lastUpdated)
                                        {
                                            ExecuteRequest(request, observer);
                                        }
                                        else
                                        {
                                            observer.OnCompleted();
                                        }
                                    }, ex => observer.OnCompleted());
                        }
                        else
                        {
                            observer.OnCompleted();
                        }
                    })
                );
        }

        private void ExecuteRequest<T>(RestfullRequest<T> request, IObserver<T> observer) where T : new()
        {
            request.Execute()
                .Subscribe(result =>
                {
                    _cache.Put(result, request.Url);
                    observer.OnNext(result);
                },
                    observer.OnError,
                    observer.OnCompleted);
        }
    }
}