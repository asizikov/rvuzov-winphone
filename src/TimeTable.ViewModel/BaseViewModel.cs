using System.ComponentModel;
using System.Windows.Threading;
using JetBrains.Annotations;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isLoading;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                SmartDispatcher.BeginInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            protected set
            {
                if (value.Equals(_isLoading)) return;
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
    }
}
