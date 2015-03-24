using System;
using System.Windows.Input;
using JetBrains.Annotations;
using TimeTable.Mvvm;

namespace TimeTable.ViewModel.Commands
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        [NotNull] private readonly Action _action;
        [CanBeNull] private readonly Func<bool> _canExecuteEvaluator;
        private readonly bool _canExecute;

        public SimpleCommand([NotNull] Action action, bool canExecute = true)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
            _canExecute = canExecute;
        }

        public SimpleCommand([NotNull] Action action, Func<bool> canExecuteEvaluator)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
            _canExecuteEvaluator = canExecuteEvaluator;
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
            return _canExecuteEvaluator != null ? _canExecuteEvaluator() : _canExecute;
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