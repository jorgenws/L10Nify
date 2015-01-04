using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class MissingLocalizedTextsViewModel : PropertyChangedBase,
                                                  IMissingLocalizedTextsViewModel {
        public IEnumerable<MissingLocalizedTextViewModel> MissingLocalizedTexts {
            get {
                return _queryModel.RetriveMissingLocalizedTexts()
                                  .Select(c => _missingLocalizedTextViewModelFactory.Create(c,
                                                                                            _queryModel));
            }
        }

        private readonly IQueryModel _queryModel;
        private readonly IMissingLocalizedTextViewModelFactory _missingLocalizedTextViewModelFactory;
        private readonly IAreaViewModelFactory _areaViewModelFactory;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICommandInvoker _commandInvoker;
        private readonly IWindowManager _windowManager;

        public MissingLocalizedTextsViewModel(IQueryModel queryModel,
                                              IMissingLocalizedTextViewModelFactory missingLocalizedTextViewModelFactory,
                                              IAreaViewModelFactory areaViewModelFactory,
                                              IGuidGenerator guidGenerator,
                                              ICommandInvoker commandInvoker,
                                              IWindowManager windowManager) {
            _queryModel = queryModel;
            _missingLocalizedTextViewModelFactory = missingLocalizedTextViewModelFactory;
            _areaViewModelFactory = areaViewModelFactory;
            _guidGenerator = guidGenerator;
            _commandInvoker = commandInvoker;
            _windowManager = windowManager;

            _queryModel.ModelHasChanged += (s, e) => RefreshView();
        }

        public void AddMissingText(MissingLocalizedTextViewModel missingLocalizedText) {
            var vm = new AddLocalizedTextViewModel(_queryModel,
                                                   _areaViewModelFactory);
            vm.LanguageId = missingLocalizedText.LanguageId;
            vm.AreaId = missingLocalizedText.AreaId;
            vm.KeyId = missingLocalizedText.KeyId;

            var dialogResult = _windowManager.ShowDialog(vm);
            if (dialogResult.HasValue && dialogResult.Value)
                _commandInvoker.Invoke(new AddLocalizedTextCommand(vm.AreaId,
                                                                   vm.KeyId,
                                                                   _guidGenerator.Next(),
                                                                   vm.LanguageId,
                                                                   vm.Text));
        }

        public void RefreshView() {
            NotifyOfPropertyChange(() => MissingLocalizedTexts);
        }
    }

    public interface IMissingLocalizedTextsViewModel {
        IEnumerable<MissingLocalizedTextViewModel> MissingLocalizedTexts { get; }
    }
}