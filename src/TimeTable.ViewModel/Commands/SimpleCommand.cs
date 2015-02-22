using System;
using System.Windows.Input;
using System.Windows.Threading;
using JetBrains.Annotations;
using TimeTable.ViewModel.Utils;

namespace TimeTable.ViewModel.Commands
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        [NotNull]
        private readonly Action _action;
        [CanBeNull]
        private readonly Func<bool> _canExecuteEvaluetor;
        private readonly bool _canExecute;

        public SimpleCommand([NotNull] Action action, bool canExecute = true)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
            _canExecute = canExecute;
        }

        public SimpleCommand([NotNull] Action action, Func<bool> canExecuteEvaluetor)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
            _canExecuteEvaluetor = canExecuteEvaluetor;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                SmartDispatcher.BeginInvoke(() => CanExecuteChanged(this, new EventArgs()));
               
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteEvaluetor != null ? _canExecuteEvaluetor() : _canExecute;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(null))
            {
                _action();
            }
            
        }
    }
}
