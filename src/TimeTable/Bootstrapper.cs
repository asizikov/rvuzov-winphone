using System;
using JetBrains.Annotations;
using Microsoft.Phone.Controls;
using TimeTable.IoC;
using TimeTable.Mvvm;

namespace TimeTable
{
    public static class Bootstrapper
    {
        public static void InitApplication([NotNull] PhoneApplicationFrame rootFrame)
        {
            if (rootFrame == null) throw new ArgumentNullException("rootFrame");

            SmartDispatcher.Initialize();
            RegisterDependencies(rootFrame);
            rootFrame.UriMapper = Container.Resolve<TimeTableUriMapper>();
        }


        private static void RegisterDependencies(PhoneApplicationFrame rootFrame)
        {
            Container.Initialize(rootFrame);
        }
    }
}