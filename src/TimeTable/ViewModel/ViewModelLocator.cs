﻿using TimeTable.ViewModel.Data;
using TimeTable.ViewModel.Services;
using TinyIoC;

namespace TimeTable.ViewModel
{
    public static class ViewModelLocator
    {
        private static TinyIoCContainer Container
        {
            get
            {
                return TinyIoCContainer.Current;
            }
        }

        public static BaseViewModel GetTmpViewModel()
        {
            return new TmpViewModel(Container.Resolve<INavigationService>(), Container.Resolve<BaseApplicationSettings>(), new AsyncDataProvider());
        }

        public static BaseViewModel GetFirstPageViewModel()
        {
            return new FirstPageViewModel(Container.Resolve<INavigationService>(), Container.Resolve<BaseApplicationSettings>());
        }

        public static BaseViewModel GetGroupstPageViewModel(int universityId)
        {
            return new GroupPageViewModel(Container.Resolve<INavigationService>(), Container.Resolve<BaseApplicationSettings>(), new AsyncDataProvider(), universityId);
        }
    }
}
