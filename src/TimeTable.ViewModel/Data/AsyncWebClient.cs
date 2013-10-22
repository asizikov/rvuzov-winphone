using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TimeTable.Model;

namespace TimeTable.ViewModel.Data
{
    public class AsyncWebClient : BaseAsyncWebClient
    {
        public AsyncWebClient([NotNull] IWebCache cache) : base(cache)
        {
        }

        public IObservable<Confirmation> PostErrorMessageAsync(int id, int lessonId, bool isTeacher, string errorText)
        {
            var error = new ErrorMessage
            {
                ErrorText = errorText,
                Lesson = new ErrorLesson
                {
                    Id = lessonId
                }
            };
            var body = JsonConvert.SerializeObject(error);
            var request = CallFactory.CreateSendErrorRequest(id, isTeacher, body);
            return GetDataAsync(request);
        }
    }
}