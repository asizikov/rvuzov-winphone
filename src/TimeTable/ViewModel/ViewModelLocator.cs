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
        public static BaseViewModel GetUniversitiesViewModel(bool isAddingFavorites)
        {
            return _universitiesViewModel ??
                   (_universitiesViewModel =
                       new UniversitiesViewModel(C.Resolve<INavigationService>(),
                           C.Resolve<BaseApplicationSettings>(), DataProvider,
                           C.Resolve<FlurryPublisher>(), isAddingFavorites));
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(C.Resolve<INavigationService>(), C.Resolve<BaseApplicationSettings>());
        }

        public static BaseViewModel GetGroupsPageViewModel(int facultyId, int universityId, bool isAddingFavorites)
        {
            return new GroupPageViewModel(C.Resolve<INavigationService>(),
                C.Resolve<BaseApplicationSettings>(),
                DataProvider,
                C.Resolve<FlurryPublisher>(), C.Resolve<FavoritedItemsManager>() ,universityId, facultyId, isAddingFavorites);
        }

        public static BaseViewModel GetFacultiesPageViewModel(int universityId, bool isAddingFavorites)
        {
            return new FacultiesPageViewModel(C.Resolve<INavigationService>(),
                C.Resolve<BaseApplicationSettings>(),
                DataProvider,
                C.Resolve<FlurryPublisher>(), universityId, isAddingFavorites);
        }

        public static LessonsViewModel GetLessonsViewModel(int id, bool isTeacher, int universityId, int facultyId)
        {
            return new LessonsViewModel(C.Resolve<INavigationService>(), C.Resolve<FlurryPublisher>(),
                C.Resolve<BaseApplicationSettings>(), C.Resolve<ICommandFactory>(),
                DataProvider, C.Resolve<FavoritedItemsManager>(),C.Resolve<IUiStringsProviders>(), id, isTeacher, universityId, facultyId);
        }

        public static BaseViewModel GetFavoritesViewModel()
        {
            return new FavoritesViewModel(C.Resolve<INavigationService>(), C.Resolve<FavoritedItemsManager>(),
                C.Resolve<IUiStringsProviders>());
        }
        public static BaseViewModel GetSettingsViewModel()
        {
            return new SettingsViewModel(C.Resolve<BaseApplicationSettings>(), C.Resolve<INavigationService>());
        }
    }
}