using System;
using System.Windows.Input;

namespace Beeffective.Presentation.Common
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> myCanExecute;
        private readonly Action<object> myExecute;

        public DelegateCommand(Action<object> execute)
            : this(o => true, execute)
        {
        }

        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            myCanExecute = canExecute;
            myExecute = execute;
        }

        public bool CanExecute(object parameter) => myCanExecute(parameter);

        public void Execute(object parameter)
        {
            if (myCanExecute(parameter) == false)
            {
                throw new InvalidOperationException();
            }
            myExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
