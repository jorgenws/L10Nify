using System;
using System.Windows.Input;

namespace L10Nify {
    public class RelayCommand : ICommand {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute,
                                                          () => true) {
            _execute = execute;
        }

        public RelayCommand(Action execute,
                            Func<bool> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter) {
            _execute.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}