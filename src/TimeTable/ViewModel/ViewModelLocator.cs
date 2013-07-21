using Funq;
using JetBrains.Annotations;
using TimeTable.IoC;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        private static Container C
        {
            get
            {
                return ContainerInstance.Current;
            }
        }

        private static UniversitiesViewModel _universitiesViewModel;

        [NotNull] public static BaseViewModel GetTmpViewModel()
        {
            return _universitiesViewModel ??
                   (_universitiesViewModel =
                       new UniversitiesViewModel(C.Resolve<INavigationService>(),
                           C.Resolve<BaseApplicationSettings>(), new AsyncDataProvider(C.Resolve<ICache>()), 
                           C.Resolve<FlurryPublisher>()));
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(C.Resolve<INavigationService>(), C.Resolve<BaseApplicationSettings>());
        }

        public static BaseViewModel GetGroupstPageViewModel(int universityId)
        {
            return new GroupPageViewModel(C.Resolve<INavigationService>(), C.Resolve<BaseApplicationSettings>(), new AsyncDataProvider(C.Resolve<ICache>()), universityId);
        }

        public static BaseViewModel GetLessonsViewModel(int groupId, string groupName)
        {
            return new LessonsViewModel(C.Resolve<INavigationService>(), 
                C.Resolve<BaseApplicationSettings>(), 
                new AsyncDataProvider(C.Resolve<ICache>()), groupId, groupName);
        }
    }
}
