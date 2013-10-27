using Funq;
using JetBrains.Annotations;
using TimeTable.IoC;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        private static Container C
        {
            get { return ContainerInstance.Current; }
        }

        private static readonly AsyncDataProvider DataProvider;

        static ViewModelLocator()
        {
            DataProvider = C.Resolve<AsyncDataProvider>();
        }

        [NotNull]
        public static BaseViewModel GetUniversitiesViewModel(Reason reason)
        {
            return
                new UniversitiesViewModel(C.Resolve<INavigationService>(),
                    C.Resolve<BaseApplicationSettings>(), DataProvider,
                    C.Resolve<FlurryPublisher>(), C.Resolve<INotificationService>(), reason);
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(C.Resolve<INavigationService>(), C.Resolve<BaseApplicationSettings>(),
                DataProvider, C.Resolve<FlurryPublisher>());
        }

        public static BaseViewModel GetGroupsPageViewModel(int facultyId, int universityId, Reason reason)
        {
            return new GroupPageViewModel(C.Resolve<INavigationService>(),
                C.Resolve<BaseApplicationSettings>(),
                DataProvider, C.Resolve<INotificationService>(),
                C.Resolve<FlurryPublisher>(), C.Resolve<FavoritedItemsManager>(), universityId, facultyId,
                reason);
        }

        public static BaseViewModel GetFacultiesPageViewModel(int universityId, Reason reason)
        {
            return new FacultiesPageViewModel(C.Resolve<INavigationService>(),
                C.Resolve<BaseApplicationSettings>(),
                DataProvider,
                C.Resolve<FlurryPublisher>(), C.Resolve<INotificationService>(), universityId, reason);
        }

        public static LessonsViewModel GetLessonsViewModel(int id, bool isTeacher, int universityId, int facultyId)
        {
            return new LessonsViewModel(C.Resolve<INavigationService>(), C.Resolve<FlurryPublisher>(),
                C.Resolve<BaseApplicationSettings>(), C.Resolve<ICommandFactory>(),
                DataProvider, C.Resolve<FavoritedItemsManager>(), C.Resolve<IUiStringsProviders>(), C.Resolve<INotificationService>(), id, isTeacher,
                universityId, facultyId);
        }

        public static AuditoriumViewModel GetAuditoriumViewModel(int auditoiumId,int universityId ,string auditoriumName,
            string auditoriumAddress)
        {
            return new AuditoriumViewModel(C.Resolve<INavigationService>(), DataProvider,
                C.Resolve<IUiStringsProviders>(), auditoiumId, universityId, auditoriumName, auditoriumAddress);
        }

        public static BaseViewModel GetFavoritesViewModel()
        {
            return new FavoritesViewModel(C.Resolve<INavigationService>(), C.Resolve<FavoritedItemsManager>(),
                C.Resolve<IUiStringsProviders>(), C.Resolve<FlurryPublisher>());
        }

        public static BaseViewModel GetSettingsViewModel()
        {
            return new SettingsViewModel(C.Resolve<BaseApplicationSettings>(), C.Resolve<INavigationService>(),
                C.Resolve<FlurryPublisher>());
        }

        public static BaseViewModel GetReportErrorViewModel(int id, int lessonId, bool isTeacher)
        {
            return new ReportErrorViewModel(C.Resolve<FlurryPublisher>(), id, lessonId,
                isTeacher,
                new AsyncWebClient(new NoCache()), C.Resolve<INotificationService>(), C.Resolve<IUiStringsProviders>());
        }

        public static BaseViewModel GetAboutViewModel()
        {
            return new AboutViewModel(C.Resolve<FlurryPublisher>());
        }
    }
}