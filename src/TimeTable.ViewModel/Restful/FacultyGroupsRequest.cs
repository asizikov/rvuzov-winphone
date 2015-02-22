using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class FacultyGroupsRequest : RestfullRequest<Groups>
    {
        public FacultyGroupsRequest(string parameters, WebService webService): base( webService)
        {
            ResourceUrl = parameters;
        }
    }
}