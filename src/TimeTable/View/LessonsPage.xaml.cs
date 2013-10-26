﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Navigation;
using TimeTable.Utils;
using TimeTable.ViewModel;
using TimeTable.ViewModel.Services;

namespace TimeTable.View
{
    public partial class LessonsPage
    {
        public LessonsPage()
        {
            InitializeComponent();
            BindableApplicationBar.Buttons.Clear();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BindableApplicationBar.Buttons.Clear();
            ApplicationBar.Buttons.Clear();

            base.OnNavigatedTo(e);

            string rawGroupId;
            string rawIsTeacher;
            string rawUniversityId;
            var rawFacultyId = string.Empty;
            if (NavigationContext.QueryString.TryGetValue(NavigationParameterName.Id, out rawGroupId)
                && NavigationContext.QueryString.TryGetValue(NavigationParameterName.IsTeacher, out rawIsTeacher)
                && NavigationContext.QueryString.TryGetValue(NavigationParameterName.UniversityId, out rawUniversityId)
                )
            {
                int id;
                if (Int32.TryParse(rawGroupId, out id))
                {
                    var isTeacher = Boolean.Parse(rawIsTeacher);
                    if (!isTeacher)
                    {
                        if (!NavigationContext.QueryString.TryGetValue(NavigationParameterName.FacultyId,
                            out rawFacultyId))
                        {
                            throw new KeyNotFoundException("LessonsPage::failed to get facultyId from QueryString");
                        }
                    }
                    DataContext = ViewModelLocator.GetLessonsViewModel(id, isTeacher,
                        Int32.Parse(rawUniversityId), isTeacher ? -1 : Int32.Parse(rawFacultyId));
                    if (State.Count > 0)
                    {
                        this.RestoreState(Pivot);
                    }
                }
            }
            else
            {
                Debug.WriteLine("LessonsPage::failed to get parameters from QueryString");
                throw new KeyNotFoundException("LessonsPage::failed to get parameters from QueryString");
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (OptionsList.Visibility == System.Windows.Visibility.Visible)
            {
                OptionsList.Visibility = System.Windows.Visibility.Collapsed;
                e.Cancel = true;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            State.Clear();
            if (this.ShouldTombstone(e))
            {
                this.SaveState(Pivot);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            BindableApplicationBar.MenuItemsSource = null;
        }
    }
}