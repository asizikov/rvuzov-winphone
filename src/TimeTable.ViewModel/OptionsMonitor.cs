﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using TimeTable.ViewModel.Commands;

namespace TimeTable.ViewModel
{
    [DebuggerDisplay("OptionsItem Title = {Title}")]
    public class OptionsItem
    {
        public string Title { get; set; }
        public ITitledCommand Command { get; set; }
    }

    public  class OptionsMonitor: BaseViewModel
    {
        private bool _isVisible;
        public ObservableCollection<OptionsItem> Items { get; set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value.Equals(_isVisible)) return;
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }
    }
}
