using System.Globalization;
using TimeTable.Model;
using TimeTable.Networking;
using TimeTable.Networking.Restful;

namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        private readonly WebService _webService = new WebService(URL_PREFIX);

        private const string URL_PREFIX = "http://new.raspisaniye-vuzov.ru/api/v1/";
        private const string UNIVERSITIES_ALL = "universities";
        private const string LAST_UPDATED = "/lastupdated";

        private const string ALL_FACULTIES_TEMPLATE = "universities/{0}/faculties";
        private const string ALL_GROUPS_TEMPLATE = "faculties/{0}/groups";
        private const string ALL_TEACHERS_TEMPLATE = "universities/{0}/teachers";
        private const string GROUP_TIMETABLE_TEMPLATE = "groups/{0}";
        private const string GROUP_ERROR_TEMPLATE = "groups/{0}/link-bug";
        private const string TEACHER_TIMETABLE_TEMPLATE = "teachers/{0}";
        private const string TEACHER_ERROR_TEMPLATE = "teachers/{0}/link-bug";

        private static string InjectIdToTemplate(string template, int resourceId)
        {
            return string.Format(template, resourceId.ToString(CultureInfo.InvariantCulture));
        }

        public RestfullRequest<Universities> GetAllUniversitesRequest()
        {
            return RestfullRequest.Create<Universities>(UNIVERSITIES_ALL, _webService);
        }

        public RestfullRequest<Groups> GetFacultyGroupsRequest(int facultyId)
        {
            return RestfullRequest.Create<Groups>(InjectIdToTemplate(ALL_GROUPS_TEMPLATE, facultyId), _webService);
        }

        public RestfullRequest<Model.TimeTable> GetGroupTimeTableRequest(int groupId)
        {
            var suffix = string.Format(GROUP_TIMETABLE_TEMPLATE, groupId);
            return RestfullRequest.Create<Model.TimeTable>(suffix, _webService);
        }

        public RestfullRequest<Model.TimeTable> GetTeacherTimeTableRequest(int id)
        {
            var suffix = string.Format(TEACHER_TIMETABLE_TEMPLATE, id);
            return RestfullRequest.Create<Model.TimeTable>(suffix, _webService);
        }

        public RestfullRequest<Teachers> GetUniversityTeachersRequest(int universityId)
        {
            return RestfullRequest.Create<Teachers>(InjectIdToTemplate(ALL_TEACHERS_TEMPLATE, universityId), _webService);
        }

        public RestfullRequest<Updates> GetLastUpdatedRequest<T>(string url)
        {
            return RestfullRequest.Create<Updates>(url + LAST_UPDATED, _webService);
        }

        public RestfullRequest<Faculties> GetUniversityFacultiesRequest(int universityId)
        {
            return RestfullRequest.Create<Faculties>(InjectIdToTemplate(ALL_FACULTIES_TEMPLATE, universityId), _webService);
        }

        public RestfullRequest<Confirmation> CreateSendErrorRequest(int id, bool isTeacher, string body)
        {
            var suffix = InjectIdToTemplate((isTeacher ? TEACHER_ERROR_TEMPLATE : GROUP_ERROR_TEMPLATE), id);
            return new SendErrorRequest(_webService, suffix , body);
        }
    }
}
