using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class LastUpdatedRequest : RestfullRequest<Updates>
    {
        public LastUpdatedRequest([NotNull] string baseUrl, string parameters, [NotNull] WebService webService) : base(baseUrl, webService)
        {
            AdditionalUrl = parameters;
        }
    }
}