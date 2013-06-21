using System;
using System.Windows.Input;
using JetBrains.Annotations;

namespace TimeTable.ViewModel.Commands
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        [NotNull]
        private readonly Action action;

        private readonly bool canExecute;

        public SimpleCommand([NotNull] Action action, bool canExecute = true)
        {
            if (action == null) throw new ArgumentNullException("action");
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
