using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

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
        private const string TEACHER_TIMETABLE_TEMPLATE = "teachers/{0}";

        private static string InjectIdToTemplate(string template, int universityId)
        {
            return string.Format(template, universityId);
        }

        public UniversitesRequest GetAllUniversitesRequest()
        {
            return new UniversitesRequest(URL_PREFIX, UNIVERSITIES_ALL, _webService);
        }

        public UniversitiesGroupsRequest GetUniversitesGroupsRequest(int universityId)
        {
            return new UniversitiesGroupsRequest(URL_PREFIX, InjectIdToTemplate(ALL_GROUPS_TEMPLATE, universityId), _webService);
        }

        public TimeTableRequest GetGroupTimeTableRequest(int groupId)
        {
            var suffix = string.Format(GROUP_TIMETABLE_TEMPLATE, groupId);
            return new TimeTableRequest(URL_PREFIX, suffix, _webService);
        }

        public TimeTableRequest GetTeacherTimeTableRequest(int id)
        {
            var suffix = string.Format(TEACHER_TIMETABLE_TEMPLATE, id);
            return new TimeTableRequest(URL_PREFIX, suffix, _webService);
        }

        public UniversityTeachersRequest GetUniversityTeachersRequest(int universityId)
        {
            return new UniversityTeachersRequest(URL_PREFIX, InjectIdToTemplate(ALL_TEACHERS_TEMPLATE, universityId), _webService );
        }

        public LastUpdatedRequest GetLastUpdatedRequest<T>(string url)
        {
            return new LastUpdatedRequest("", url + LAST_UPDATED , _webService);
        }
    }
}
