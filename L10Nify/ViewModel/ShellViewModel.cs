using System.Windows.Forms;
using System.Windows.Input;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class ShellViewModel : PropertyChangedBase,
                                  IShell {
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }

        private IWorkbench _workbench;
        public IWorkbench Workbench {
            get { return _workbench; }
            set {
                _workbench = value;
                NotifyOfPropertyChange(() => Workbench);
            }
        }

        private readonly IQueryModel _queryModel;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IWorkbenchFactory _workbenchFactory;
        private readonly IEventAggregator _eventAggregator;

        public ShellViewModel(IQueryModel queryModel,
                              ICommandInvoker commandInvoker,
                              IWorkbenchFactory workbenchFactory,
                              IEventAggregator eventAggregator) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _workbenchFactory = workbenchFactory;
            _eventAggregator = eventAggregator;

            UndoCommand = new RelayCommand(Undo);
            RedoCommand = new RelayCommand(Redo);
            NewCommand = new RelayCommand(New);
            OpenCommand = new RelayCommand(Open);
            SaveCommand = new RelayCommand(Save,
                                           () => _queryModel.HasFileName());
            SaveAsCommand = new RelayCommand(SaveAs);

            _workbench = _workbenchFactory.Create(WorkbenchType.ListOriented);
        }

        public void Undo() {
            _commandInvoker.Undo();
        }

        public void Redo() {
            _commandInvoker.Do();
        }

        public void New() {
            _commandInvoker.Invoke(new NewCommand());
            CommandManager.InvalidateRequerySuggested();
            _eventAggregator.PublishOnCurrentThread(new ModelIsUpdatedFromFile());
        }

        public void Open() {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK) {
                _commandInvoker.Invoke(new LoadCommand(ofd.FileName));
                CommandManager.InvalidateRequerySuggested();
                _eventAggregator.PublishOnCurrentThread(new ModelIsUpdatedFromFile());
            }
        }

        public void Save() {
            _commandInvoker.Invoke(new SaveCommand());
        }

        public void SaveAs() {
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK) {
                _commandInvoker.Invoke(new SaveAsCommand(sfd.FileName));
                CommandManager.InvalidateRequerySuggested();
                _eventAggregator.PublishOnCurrentThread(new ModelIsUpdatedFromFile());
            }
        }
    }

    public class ModelIsUpdatedFromFile {}
}