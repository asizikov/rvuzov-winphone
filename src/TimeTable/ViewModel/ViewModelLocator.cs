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

        private static UniversitiesViewModel _universitiesViewModel;
        private static readonly AsyncDataProvider DataProvider;

        static ViewModelLocator()
        {
            DataProvider = new AsyncDataProvider(C.Resolve<ICache>());
        }

        [NotNull]
        public static BaseViewModel GetUniversitiesViewModel()
        {
            return _universitiesViewModel ??
                   (_universitiesViewModel =
                       new UniversitiesViewModel(C.Resolve<INavigationService>(),
                           C.Resolve<BaseApplicationSettings>(), DataProvider,
                           C.Resolve<FlurryPublisher>()));
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(C.Resolve<INavigationService>(), C.Resolve<BaseApplicationSettings>());
        }

        public static BaseViewModel GetGroupsPageViewModel(int facultyId, int universityId)
        {
            return new GroupPageViewModel(C.Resolve<INavigationService>(),
                C.Resolve<BaseApplicationSettings>(),
                DataProvider,
                C.Resolve<FlurryPublisher>(), universityId, facultyId);
        }

        public static BaseViewModel GetFacultiesPageViewModel(int universityId)
        {
            return new FacultiesPageViewModel(C.Resolve<INavigationService>(),
                C.Resolve<BaseApplicationSettings>(),
                DataProvider,
                C.Resolve<FlurryPublisher>(), universityId);
        }

        public static BaseViewModel GetLessonsViewModel(int id, bool isTeacher, int universityId, int facultyId)
        {
            return new LessonsViewModel(C.Resolve<INavigationService>(), C.Resolve<FlurryPublisher>(),
                C.Resolve<BaseApplicationSettings>(), C.Resolve<ICommandFactory>(),
                DataProvider, C.Resolve<FavoritedItemsManager>(), id, isTeacher, universityId, facultyId);
        }

        public static BaseViewModel GetFavoritesViewModel()
        {
            return new FavoritesViewModel(C.Resolve<INavigationService>(), C.Resolve<FavoritedItemsManager>(),
                C.Resolve<IUiStringsProviders>());
        }
    }
}