﻿using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using TimeTable.Mvvm;
using TimeTable.Utils;
using TimeTable.ViewModel.OrganizationalStructure;

namespace TimeTable.View
{
    public abstract class SearchPage : PhoneApplicationPage
    {
        private ISearchViewModel _viewModel;
        protected const string SearchKey = "search_visibility_key";

        protected ISearchViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (value != null)
                {
                    _viewModel = value;
                    _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
                }
                else
                {
                    if (_viewModel == null) return;
                    _viewModel.PropertyChanged -= ViewModelOnPropertyChanged;
                    _viewModel = null;
                }
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsSearchBoxVisible")
            {
                Scheduler.Default.Schedule(TimeSpan.FromMilliseconds(200), () => SmartDispatcher.BeginInvoke(SetFocus));
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (ViewModel != null && ViewModel.IsSearchBoxVisible)
            {
                ViewModel.ResetSearchState();
                e.Cancel = true;
            }
        }

        protected abstract void SaveState(NavigatingCancelEventArgs e);

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            State.Clear();
            if (this.ShouldTombstone(e))
            {
                SaveState(e);
            }
            OnLeave();
        }

        protected virtual void OnLeave()
        {
            ViewModel = null;
        }

        protected abstract void SetFocus();
    }
}