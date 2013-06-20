using TimeTable.Model;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public sealed class UniversitiesAllRequest : RestfullRequest<UniversityAll>
    {
        public UniversitiesAllRequest(string baseUrl, string parameters) : base(baseUrl)
        {
            AdditionalUrl = parameters;
        }

        protected override string AdditionalUrl
        {
             get; 
             set;
        }
    }
}
