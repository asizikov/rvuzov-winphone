using JetBrains.Annotations;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public class TimeTableRequest : RestfullRequest<Model.TimeTable>
    {
        public TimeTableRequest(string resourceUrl, [NotNull] WebService webService) : base(webService)
        {
            ResourceUrl = resourceUrl;
        }
    }
}