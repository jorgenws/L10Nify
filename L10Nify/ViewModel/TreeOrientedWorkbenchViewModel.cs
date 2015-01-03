using System.Collections.Generic;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class TreeOrientedWorkbenchViewModel : PropertyChangedBase, IWorkbench, IHandle<ModelIsUpdatedFromFile> {
        private IEnumerable<ITreeNodeViewModel> _tree;
        public IEnumerable<ITreeNodeViewModel> Tree {
            get { return _tree; }
            private set {
                _tree = value;
                NotifyOfPropertyChange(() => Tree);
            }
        }

        private readonly IQueryModel _queryModel;
        private readonly ICommandInvoker _commandInvoker;
        private readonly ITreeViewModelBuilder _treeViewModelBuilder;
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;

        public TreeOrientedWorkbenchViewModel(IQueryModel queryModel,
                                              ICommandInvoker commandInvoker,
                                              ITreeViewModelBuilder treeViewModelBuilder,
                                              IWindowManager windowManager,
                                              IEventAggregator eventAggregator) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _treeViewModelBuilder = treeViewModelBuilder;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            RefreshView();
        }

        public void RefreshView() {
            Tree = _treeViewModelBuilder.Build(_queryModel);
        }

        public void Handle(ModelIsUpdatedFromFile message) {
            RefreshView();
        }
    }
}