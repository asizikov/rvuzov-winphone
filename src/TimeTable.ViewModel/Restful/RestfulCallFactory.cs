using TimeTable.Networking;

namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        private readonly WebService _webService = new WebService();

        private const string URL_PREFIX = "http://raspisanie-vuzov.ru/api/v1/";
        private const string UNIVERSITIES_ALL = "universities-all";
        private const string LAST_UPDATED = "/last-updated";
        private const string ALL_GROUPS_TEMPLATE = "universities/{0}/groups-all";
        private const string ALL_TEACHERS_TEMPLATE = "universities/{0}/teachers-all";
        private const string GROUP_TIMETABLE_TEMPLATE = "groups/{0}";

        private static string InjectIdToTemplate(string template, int universityId)
        {
            return string.Format(template, universityId);
        }

        //TODO: create calls for all API endpoints
        //universities/{id}/groups-all/last-updated list all groups for specified university last updated time
        //universities/{id}/teachers-all list all teachers for specified university
        //universities/{id}/teachers-all/last-updated list all teachers for specified university last updated time
        
        //groups/{id}/last-updated list specified group schedule last updated time
        //teachers/{id} list specified teacher schedule
        //teachers/{id}/last-updated list specified teacher schedule last updated time

        public UniversitesRequest GetAllUniversitesRequest()
        {
            return new UniversitesRequest(URL_PREFIX, UNIVERSITIES_ALL, _webService);
        }

        public UniversitiesGroupsRequest GetUniversitesGroupsRequest(int universityId)
        {
            return new UniversitiesGroupsRequest(URL_PREFIX, InjectIdToTemplate(ALL_GROUPS_TEMPLATE, universityId), _webService);
        }

        public GroupTimeTableRequest GetGroupTimeTableRequest(int groupId)
        {
            var suffix = string.Format(GROUP_TIMETABLE_TEMPLATE, groupId);
            return new GroupTimeTableRequest(URL_PREFIX, suffix, _webService);
        }

        public UniversityTeachersRequest GetUniversityTeachersRequest(int universityId)
        {
            return new UniversityTeachersRequest(URL_PREFIX, InjectIdToTemplate(ALL_TEACHERS_TEMPLATE, universityId), _webService );
        }

        public LastUpdatedRequest GetLastUpdatedRequest<T>()
        {
            return new LastUpdatedRequest(URL_PREFIX, UNIVERSITIES_ALL + LAST_UPDATED, _webService);
        }
    }
}
