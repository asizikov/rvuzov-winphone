using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversityTeachersRequest : RestfullRequest<Teachers>
    {
        public UniversityTeachersRequest(string baseUrl, string parameters, WebService webService)
            : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }
    }
}