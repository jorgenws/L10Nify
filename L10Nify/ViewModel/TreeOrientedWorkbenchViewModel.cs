using System.Collections.Generic;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class TreeOrientedWorkbenchViewModel : PropertyChangedBase, IWorkbench {
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

        public TreeOrientedWorkbenchViewModel(IQueryModel queryModel,
                                              ICommandInvoker commandInvoker,
                                              ITreeViewModelBuilder treeViewModelBuilder,
                                              IWindowManager windowManager) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _treeViewModelBuilder = treeViewModelBuilder;
            _windowManager = windowManager;

            _queryModel.ModelHasChanged += (s, e) => RefreshView();

            RefreshView();
        }

        public void RefreshView() {
            Tree = _treeViewModelBuilder.Build(_queryModel);
        }
    }
}