using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Caliburn.Micro;
using Core;
using Screen = Caliburn.Micro.Screen;

namespace L10Nify {
    public class ShellViewModel : PropertyChangedBase,
                                  IShell {
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }

        public IEnumerable<string> Languages {
            get {
                return _queryModel.RetriveLanguages()
                                  .Select(c => c.DisplayName)
                                  .ToList();
            }
        }

        public IEnumerable<AreaViewModel> Areas {
            get {
                return _queryModel.RetriveAreas()
                                  .Select(c => _areaViewModelFactory.Create(c))
                                  .ToList();
            }
        }

        public IEnumerable<string> Keys {
            get {
                return _queryModel.RetriveLocalizationKeys()
                                  .Select(c => c.Key)
                                  .ToList();
            }
        }

        public IEnumerable<string> Texts {
            get {
                return _queryModel.RetriveLocalizedTexts()
                                  .Select(c => c.Text)
                                  .ToList();
            }
        } 


        private readonly IQueryModel _queryModel;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IAreaViewModelFactory _areaViewModelFactory;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(IQueryModel queryModel,
                              ICommandInvoker commandInvoker,
                              IGuidGenerator guidGenerator,
                              IAreaViewModelFactory areaViewModelFactory,
                              IWindowManager windowManager) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _guidGenerator = guidGenerator;
            _areaViewModelFactory = areaViewModelFactory;
            _windowManager = windowManager;

            UndoCommand = new RelayCommand(Undo);
            RedoCommand = new RelayCommand(Redo);
            NewCommand = new RelayCommand(New);
            OpenCommand = new RelayCommand(Open);
            SaveCommand = new RelayCommand(Save,
                                           () => _queryModel.HasFileName());
            SaveAsCommand = new RelayCommand(SaveAs);
        }

        public void AddLanguage() {
            var vm = new AddLanguageViewModel();
            OpenDataView(vm,
                         c => new AddLanguageCommand(_guidGenerator.Next(),
                                                     c.IsoName,
                                                     c.LanguageDisplayName));
        }

        public void AddArea() {
            var vm = new AddAreaViewModel();
            OpenDataView(vm,
                         c => new AddAreaCommand(_guidGenerator.Next(),
                                                 c.AreaName,
                                                 c.Comment,
                                                 c.Image));
        }

        public void AddLocalizationKey() {
            var vm = new AddLocalizationKeyViewModel(_queryModel);
            OpenDataView(vm,
                         c => new AddLocalizationKeyCommand(c.AreaId,
                                                            _guidGenerator.Next(),
                                                            c.KeyName));
        }

        public void AddLocalizedText() {
            var vm = new AddLocalizedTextViewModel(_queryModel);
            OpenDataView(vm,
                         c => new AddLocalizedTextCommand(c.AreaId,
                                                          c.KeyId,
                                                          _guidGenerator.Next(),
                                                          c.LanguageId,
                                                          c.Text));
        }

        private void OpenDataView<T>(T vm, Func<T, BaseCommand> createCommand) where T : Screen {
            var result = _windowManager.ShowDialog(vm);
            if (result.HasValue && result.Value) {
                _commandInvoker.Invoke(createCommand(vm));
                RefreshView();
            }
        }

        public void Undo() {
            _commandInvoker.Undo();
            RefreshView();
        }

        public void Redo() {
            _commandInvoker.Do();
            RefreshView();
        }

        public void New() {
            _commandInvoker.Invoke(new NewCommand());
            RefreshView();
            CommandManager.InvalidateRequerySuggested();
        }

        public void Open() {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK) {
                _commandInvoker.Invoke(new LoadCommand(ofd.FileName));
                RefreshView();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public void Save() {
            _commandInvoker.Invoke(new SaveCommand());
        }

        public void SaveAs() {
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK) {
                _commandInvoker.Invoke(new SaveAsCommand(sfd.FileName));
                RefreshView();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void RefreshView() {
            NotifyOfPropertyChange(() => Languages);
            NotifyOfPropertyChange(() => Areas);
            NotifyOfPropertyChange(() => Keys);
            NotifyOfPropertyChange(() => Texts);
        }
    }
}