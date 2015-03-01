using JetBrains.Annotations;
using TimeTable.IoC;
using TimeTable.Mvvm;
using TimeTable.ViewModel.ApplicationLevel;
using TimeTable.ViewModel.FavoritedTimeTables;
using TimeTable.ViewModel.OrganizationalStructure;
using TimeTable.ViewModel.WeekOverview;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {

        [NotNull]
        public static BaseViewModel GetUniversitiesViewModel(Reason reason)
        {
            var vm = Container.Resolve<UniversitiesPageViewModel>();
            vm.Initialize(reason);
            return vm;
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return Container.Resolve<FirstPageViewModel>();
        }

        public static BaseViewModel GetGroupsPageViewModel(NavigationFlow navigationFlow)
        {
            var vm = Container.Resolve<GroupPageViewModel>();
            vm.Initialize(navigationFlow);
            return vm;
        }

        public static BaseViewModel GetFacultiesPageViewModel(NavigationFlow navigationFlow)
        {
            var vm = Container.Resolve<FacultiesPageViewModel>();
            vm.Initialize(navigationFlow);
            return vm;
        }

        public static LessonsPageViewModel GetLessonsViewModel(LessonsNavigationParameter navigationParameter)
        {
            var vm = Container.Resolve<LessonsPageViewModel>();
            vm.Initialize(navigationParameter);
            return vm;
        }

        public static AuditoriumViewModel GetAuditoriumViewModel(AuditoriumNavigationParameter navigationParameter)
        {
            var vm = Container.Resolve<AuditoriumViewModel>();
            vm.Initialize(navigationParameter);
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