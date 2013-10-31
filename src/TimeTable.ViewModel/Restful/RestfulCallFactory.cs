using System;
using TimeTable.Networking;

namespace TimeTable.ViewModel.Restful
{
    public class RestfulCallFactory
    {
        private readonly WebService _webService = new WebService();

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

        private static string InjectIdToTemplate(string template, int universityId)
        {
            return string.Format(template, universityId);
        }

        public UniversitesRequest GetAllUniversitesRequest()
        {
            return new UniversitesRequest(URL_PREFIX, UNIVERSITIES_ALL, _webService);
        }

        public FacultyGroupsRequest GetFacultyGroupsRequest(int facultyId)
        {
            return new FacultyGroupsRequest(URL_PREFIX, InjectIdToTemplate(ALL_GROUPS_TEMPLATE, facultyId), _webService);
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

        public UniversityFacultiesRequest GetUniversityFacultiesRequest(int universityId)
        {
            return new UniversityFacultiesRequest(URL_PREFIX, InjectIdToTemplate(ALL_FACULTIES_TEMPLATE, universityId), _webService);
        }

        public SendErrorRequest CreateSendErrorRequest(int id, bool isTeacher, string body)
        {
            var suffix = InjectIdToTemplate((isTeacher ? TEACHER_ERROR_TEMPLATE : GROUP_ERROR_TEMPLATE), id);
            return new SendErrorRequest(_webService, URL_PREFIX, suffix , body);
        }
    }
}
