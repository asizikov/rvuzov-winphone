using JetBrains.Annotations;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public class TimeTableRequest : RestfullRequest<Model.TimeTable>
    {
        public TimeTableRequest([NotNull] string baseUrl,string additionalUrl, [NotNull] WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = additionalUrl;
        }
    }
}