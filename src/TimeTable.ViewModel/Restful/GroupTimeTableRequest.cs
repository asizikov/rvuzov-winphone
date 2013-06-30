using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public class GroupTimeTableRequest : RestfullRequest<GroupTimeTable>
    {
        public GroupTimeTableRequest([NotNull] string baseUrl,string additionalUrl, [NotNull] WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = additionalUrl;
        }
    }
}