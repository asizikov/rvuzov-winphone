using JetBrains.Annotations;
using TimeTable.IoC;
using TimeTable.ViewModel.Data;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        private static readonly AsyncDataProvider DataProvider;

        static ViewModelLocator()
        {
            DataProvider = Container.Resolve<AsyncDataProvider>();
        }

        [NotNull]
        public static BaseViewModel GetUniversitiesViewModel(Reason reason)
        {
            var vm = Container.Resolve<UniversitiesViewModel>();
            vm.Initialize(reason);
            return vm;
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return Container.Resolve<FirstPageViewModel>();
        }

        public static BaseViewModel GetGroupsPageViewModel(int facultyId, int universityId, Reason reason)
        {
            var vm = Container.Resolve<GroupPageViewModel>();
            vm.Initialize(universityId, facultyId, reason);
            return vm;
        }

        public static BaseViewModel GetFacultiesPageViewModel(int universityId, Reason reason)
        {
            var vm = Container.Resolve<FacultiesPageViewModel>();
            vm.Initialize(universityId, reason);
            return vm;
        }

        public static LessonsViewModel GetLessonsViewModel(int id, bool isTeacher, int universityId, int facultyId)
        {
            var vm = Container.Resolve<LessonsViewModel>();
            vm.Initialize(id, isTeacher, universityId, facultyId);
            return vm;
        }

        public static AuditoriumViewModel GetAuditoriumViewModel(int auditoriumId, int universityId,
            string auditoriumName,
            string auditoriumAddress)
        {
            var vm = Container.Resolve<AuditoriumViewModel>();
            vm.Initialize(auditoriumId, universityId, auditoriumName, auditoriumAddress);
            return vm;
        }

        public static BaseViewModel GetFavoritesViewModel()
        {
            return Container.Resolve<FavoritesViewModel>();
        }

        public static BaseViewModel GetSettingsViewModel()
        {
            return Container.Resolve<SettingsViewModel>();
        }

        public static BaseViewModel GetAboutViewModel()
        {
            return Container.Resolve<AboutViewModel>();
        }
    }
}