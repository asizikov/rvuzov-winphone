using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class FacultyGroupsRequest : RestfullRequest<Groups>
    {
        public FacultyGroupsRequest(string baseUrl, string parameters, WebService webService): base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }
    }
}