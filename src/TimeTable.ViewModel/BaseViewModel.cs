using System.ComponentModel;
using System.Windows.Threading;
using JetBrains.Annotations;

namespace TimeTable.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                SmartDispatcher.BeginInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}
