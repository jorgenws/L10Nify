using System;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class WorkbenchFactory : IWorkbenchFactory {
        private readonly IQueryModel _queryModel;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IAreaViewModelFactory _areaViewModelFactory;
        private readonly ITreeViewModelBuilder _treeViewModelBuilder;
        private readonly IWindowManager _windowManager;

        public WorkbenchFactory(IQueryModel queryModel,
                                ICommandInvoker commandInvoker,
                                IGuidGenerator guidGenerator,
                                IAreaViewModelFactory areaViewModelFactory,
                                ITreeViewModelBuilder treeViewModelBuilder,
                                IWindowManager windowManager) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _guidGenerator = guidGenerator;
            _areaViewModelFactory = areaViewModelFactory;
            _treeViewModelBuilder = treeViewModelBuilder;
            _windowManager = windowManager;
        }

        public IWorkbench Create(WorkbenchType type) {
            if (type == WorkbenchType.ListOriented)
                return new ListOrientedWorkbenchViewModel(_queryModel,
                                                          _commandInvoker,
                                                          _guidGenerator,
                                                          _areaViewModelFactory,
                                                          _windowManager);

            if (type == WorkbenchType.TreeOriented)
                return new TreeOrientedWorkbenchViewModel(_queryModel,
                                                          _commandInvoker,
                                                          _treeViewModelBuilder,
                                                          _windowManager);

            throw new NotImplementedException(string.Format("There is no view model implemented for {0}",
                                                            type));
        }
    }

    public interface IWorkbenchFactory {
        IWorkbench Create(WorkbenchType type);
    }

    public enum WorkbenchType {
        ListOriented,
        TreeOriented
    }
}