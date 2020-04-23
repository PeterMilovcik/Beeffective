using System;
using System.Windows.Input;

namespace Beeffective.Presentation.Common
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> action;

        public DelegateCommand(Action<object> action)
            : this(obj => true, action)
        {
        }

        public DelegateCommand(Func<object, bool> canExecute, Action<object> action)
        {
            this.canExecute = canExecute;
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => canExecute(parameter);

        public void Execute(object parameter) => action(parameter);
    }
}
