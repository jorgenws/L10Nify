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

        public ICommand SetToListCommand { get; private set; }
        public ICommand SetToTreeCommand { get; private set; }

        private IWorkbench _workbench;
        public IWorkbench Workbench {
            get { return _workbench; }
            set {
                _workbench = value;
                NotifyOfPropertyChange(() => Workbench);
            }
        }

        public bool IsSetToList {
            get { return SelectedWorkbenchType == WorkbenchType.ListOriented; }
            set {}
        }

        public bool IsSetToTree {
            get { return SelectedWorkbenchType == WorkbenchType.TreeOriented; }
            set { }
        }

        private WorkbenchType _selectedWorkbenchType;
        public WorkbenchType SelectedWorkbenchType {
            get { return _selectedWorkbenchType; }
            set {
                _selectedWorkbenchType = value;
                
                Workbench = _workbenchFactory.Create(_selectedWorkbenchType);
                
                NotifyOfPropertyChange(() => IsSetToList);
                NotifyOfPropertyChange(() => IsSetToTree);
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
            SetToListCommand = new RelayCommand(SetToList);
            SetToTreeCommand = new RelayCommand(SetToTree);

            _workbench = _workbenchFactory.Create(WorkbenchType.ListOriented);
        }

        public void SetToList() {
            SelectedWorkbenchType = WorkbenchType.ListOriented;            
        }

        public void SetToTree() {
            SelectedWorkbenchType = WorkbenchType.TreeOriented;
        }

        public void Undo() {
            _commandInvoker.Undo();
            UpdateModel();
        }

        public void Redo() {
            _commandInvoker.Do();
            UpdateModel();
        }

        public void New() {
            _commandInvoker.Invoke(new NewCommand());
            CommandManager.InvalidateRequerySuggested();
            UpdateModel();
        }

        public void Open() {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK) {
                _commandInvoker.Invoke(new LoadCommand(ofd.FileName));
                CommandManager.InvalidateRequerySuggested();
                UpdateModel();
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
                UpdateModel();
            }
        }

        private void UpdateModel() {
            _eventAggregator.PublishOnCurrentThread(new ModelIsUpdatedFromFile());
        }
    }

    public class ModelIsUpdatedFromFile {}
}