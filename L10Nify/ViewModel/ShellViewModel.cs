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

        public IEnumerable<string> Areas {
            get {
                return _queryModel.RetriveAreas()
                                  .Select(c => c.Name)
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
            var result = _windowManager.ShowDialog(vm);
            if (result.HasValue && result.Value)
                _commandInvoker.Invoke(new AddLanguageCommand(_guidGenerator.Next(),
                                                              vm.IsoName,
                                                              vm.LanguageDisplayName));
            RefreshView();
        }

        public void AddArea() {
            var vm = new AddAreaViewModel();
            var result = _windowManager.ShowDialog(vm);
            if (result.HasValue && result.Value)
                _commandInvoker.Invoke(new AddAreaCommand(_guidGenerator.Next(),
                                                          vm.AreaName));

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
            NotifyOfPropertyChange(() => Areas);
        }
    }
}