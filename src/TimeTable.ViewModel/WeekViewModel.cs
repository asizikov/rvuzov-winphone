using System.Collections.Generic;
using System.Collections.ObjectModel;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Extensions;

namespace TimeTable.ViewModel
{
    public sealed class WeekViewModel : BaseViewModel
    {
        private ObservableCollection<DayViewModel> _days;
        private DayViewModel _selectedDayItem; 

        public WeekViewModel(IEnumerable<Day> days, int weekNumber, ICommandFactory commandFactory, WeekType type,
            University university, bool isTeacher, int holderId)
        {
            var parity = weekNumber%2;
            WeekNumber = weekNumber;

            Days =
                new ObservableCollection<DayViewModel>(days.ToViewModelList(commandFactory, type, parity, university, isTeacher, holderId));
              
        }

        public int WeekNumber { get; private set; }

        public ObservableCollection<DayViewModel> Days
        {
            get { return _days; }
            private set
            {
                if (Equals(value, _days)) return;
                _days = value;
                OnPropertyChanged("Days");
            }
        }

        public DayViewModel SelectedDayItem 
        {
            get {return _selectedDayItem;}
            set 
            {
                
                if(Equals(_selectedDayItem,value)) return;

                _selectedDayItem = value;
                OnPropertyChanged("SelectedDayItem");
            }
        }
    }
}