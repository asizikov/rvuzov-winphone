using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TimeTable.Mvvm.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.WeekOverview;

namespace TimeTable.View
{
    [DependsOnViewModel(typeof(LessonsPageViewModel))]
    public partial class NewLessonsPage
    {
        public NewLessonsPage()
        {
            InitializeComponent();
//            BindableApplicationBar.Buttons.Clear();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
//            BindableApplicationBar.Buttons.Clear();
//            ApplicationBar.Buttons.Clear();

            base.OnNavigatedTo(e);
            var navigationContext = NavigationContext.QueryString.RestoreContext<LessonsNavigationParameter>();
            DataContext = await ViewModelLocator.GetLessonsViewModel(navigationContext.Body);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
//            if (OptionsList.Visibility == System.Windows.Visibility.Visible)
//            {
//                OptionsList.Visibility = System.Windows.Visibility.Collapsed;
//                e.Cancel = true;
//            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            State.Clear();
            if (this.ShouldTombstone(e))
            {
//                this.SaveState(Pivot);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
//            BindableApplicationBar.MenuItemsSource = null;
        }
   
    }
}