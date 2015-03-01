using System.Diagnostics;
using System.Globalization;
using Microsoft.Phone.Controls;
using Ninject;
using TimeTable.Data;
using TimeTable.Data.Cache;
using TimeTable.Domain;
using TimeTable.Mvvm.Navigation;
using TimeTable.Networking.Cache;
using TimeTable.Resources;
using TimeTable.Services;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.FavoritedTimeTables;
using TimeTable.ViewModel.Services;

namespace TimeTable.IoC
{
    public static class Container
    {
        private static readonly IKernel Kernel = new StandardKernel();

        public static void Initialize(PhoneApplicationFrame rootFrame)
        {
            var stopwatch = Stopwatch.StartNew();
            Debug.WriteLine("Container::Initialize started");
            Kernel.Bind<PhoneApplicationFrame>().ToConstant(rootFrame);
            Kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            Kernel.Bind<IPlatformNavigationService>().To<PlatformNavigationService>().InSingletonScope();
            Kernel.Bind<BaseApplicationSettings>().To<ApplicationSettings>().InSingletonScope();
#if DEBUG
            Kernel.Bind<FlurryPublisher>().To<DebugFlurryPublisher>().InSingletonScope();
#else
            Kernel.Bind<FlurryPublisher>().To<FlurryPublisherImpl>().InSingletonScope();
#endif
            Kernel.Bind<IWebCache>().To<InMemoryCache>().InSingletonScope();
            Kernel.Bind<UniversitiesCache>().To<UniversitiesCache>().InSingletonScope();
            Kernel.Bind<IAsyncDataProvider>().To<AsyncDataProvider>().InSingletonScope();
            Kernel.Bind<IUiStringsProviders>().To<UiStringsProvider>().InSingletonScope();
            Kernel.Bind<INotificationService>().To<NotificationService>().InSingletonScope();
            Kernel.Bind<ICommandFactory>().To<CommandsFactory>().InSingletonScope();
            Kernel.Bind<FavoritedItemsManager>().To<FavoritedItemsManager>().InSingletonScope();
            Debug.WriteLine("Container::Initialize ended in {0} ms", stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public static T Resolve<T>()
        {
            return Kernel.Get<T>();
        }
    }
}
