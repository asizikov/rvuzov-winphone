using System.Threading.Tasks;
using RestSharp;

namespace TimeTable.Networking.Restful
{
    public static class RestSharpExtensions
    {
        public static Task<IRestResponse> ExecuteAwait(this IRestClient client, IRestRequest request)
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, (response, asyncHandle) =>
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    taskCompletionSource.SetException(response.ErrorException);
                }
                else
                {
                    taskCompletionSource.SetResult(response);
                }
            });
            return taskCompletionSource.Task;
        }
    }
}
