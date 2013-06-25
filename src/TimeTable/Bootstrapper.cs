using System;
using System.Windows.Threading;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using TimeTable.Services;
using TimeTable.ViewModel.Services;
using TinyIoC;

namespace TimeTable
{
    public static class Bootstrapper
    {
        public static void InitApplication([NotNull] PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null) throw new ArgumentNullException("rootFrame");

            SmartDispatcher.Initialize();
            RegisterDependencies(rootFrame);
        }


        private static void RegisterDependencies(PhoneApplicationFrame rootFrame)
        {
            var ioc = TinyIoCContainer.Current;
            ioc.Register<INavigationService>((container, overloads) => new NavigationService(rootFrame));
            ioc.Register<BaseApplicationSettings>((container, overloads) => new ApplicationSettings());
        }
    }
}
