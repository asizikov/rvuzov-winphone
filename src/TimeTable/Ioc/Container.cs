using Microsoft.Phone.Controls;
using Ninject;
using TimeTable.Data;
using TimeTable.Domain;
using TimeTable.Networking.Cache;
using TimeTable.Resources;
using TimeTable.Services;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.FavoritedTimeTables;
using TimeTable.ViewModel.Services;

namespace TimeTable.IoC
{
    public static class Container
    {
        private static readonly IKernel Kernel = new StandardKernel();

        public static void Initialize(PhoneApplicationFrame rootFrame)
        {
            Kernel.Bind<PhoneApplicationFrame>().ToConstant(rootFrame);
            Kernel.Bind<INavigationService>().To<NavigationService>();
            Kernel.Bind<BaseApplicationSettings>().To<ApplicationSettings>().InSingletonScope();
#if DEBUG
            Kernel.Bind<FlurryPublisher>().To<DebugFlurryPublisher>().InSingletonScope();
#else
            Kernel.Bind<FlurryPublisher>().To<FlurryPublisherImpl>().InSingletonScope();
#endif
            Kernel.Bind<IWebCache>().To<InMemoryCache>().InSingletonScope();
            Kernel.Bind<UniversitiesCache>().To<UniversitiesCache>().InSingletonScope();
            Kernel.Bind<IAsyncDataProvider>().To<AsyncDataProvider>().InSingletonScope();
            Kernel.Bind<IUiStringsProviders>().To<UiStringsProvider>();
            Kernel.Bind<INotificationService>().To<NotificationService>();
            Kernel.Bind<ICommandFactory>().To<CommandsFactory>();
            Kernel.Bind<FavoritedItemsManager>().To<FavoritedItemsManager>().InSingletonScope();
        }

        public static T Resolve<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
