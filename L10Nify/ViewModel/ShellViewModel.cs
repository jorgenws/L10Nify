using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class ShellViewModel : PropertyChangedBase,
                                  IShell {
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public IEnumerable<string> Languages {
            get {
                return _queryModel.RetriveLanguages()
                                  .Select(c => c.DisplayName)
                                  .ToList();
            }
        }

        private readonly IQueryModel _queryModel;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(IQueryModel queryModel,
                              ICommandInvoker commandInvoker,
                              IGuidGenerator guidGenerator,
                              IWindowManager windowManager) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _guidGenerator = guidGenerator;
            _windowManager = windowManager;

            UndoCommand = new RelayCommand(Undo);
            RedoCommand = new RelayCommand(Redo);
        }

        public void AddLanguage() {
            var vm = new AddLanguageViewModel();
            _windowManager.ShowDialog(vm);
            _commandInvoker.Invoke(new AddLanguageCommand(_guidGenerator.Next(),
                                                          vm.IsoName,
                                                          vm.LanguageDisplayName));
            RefreshView();
        }

        public void Undo() {
            _commandInvoker.Undo();
            RefreshView();
        }

        public void Redo() {
            _commandInvoker.Do();
            RefreshView();
        }

        private void RefreshView() {
            NotifyOfPropertyChange(() => Languages);
        }
    }

    public class RelayCommand : ICommand {
        private readonly System.Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(System.Action execute) : this(execute,
                                                          () => true) {
            _execute = execute;
        }

        public RelayCommand(System.Action execute,
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