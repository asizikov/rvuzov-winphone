using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using TimeTable.Model;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Extensions;

namespace TimeTable.ViewModel
{
    public sealed class WeekViewModel : BaseViewModel
    {
        private readonly WeekType _type;
        private ObservableCollection<DayViewModel> _days;
        private DayViewModel _selectedDayItem; 

        public WeekViewModel(IEnumerable<Day> days, int weekNumber, ICommandFactory commandFactory, WeekType type,
            University university)
        {
            var parity = weekNumber%2;
            _type = type;
            WeekNumber = weekNumber;

            Days =
                new ObservableCollection<DayViewModel>(days.ToViewModelList(commandFactory, _type, parity, university));
   
        }

        public int WeekNumber { get; private set; }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
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