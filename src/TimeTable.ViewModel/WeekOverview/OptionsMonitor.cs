using System.Collections.ObjectModel;

namespace TimeTable.ViewModel.WeekOverview
{
    public class OptionsMonitor : BaseViewModel
    {
        private bool _isVisible;
        private ObservableCollection<OptionsItem> _items;
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public ObservableCollection<OptionsItem> Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged("Items");
            }
        }

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