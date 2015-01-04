using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Core;

namespace L10Nify {
    public class ListOrientedWorkbenchViewModel : PropertyChangedBase,
                                                  IWorkbench {
        public IEnumerable<Language> Languages {
            get {
                return _queryModel.RetriveLanguages()
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

        public ListOrientedWorkbenchViewModel(IQueryModel queryModel,
                                              ICommandInvoker commandInvoker,
                                              IGuidGenerator guidGenerator,
                                              IAreaViewModelFactory areaViewModelFactory,
                                              IWindowManager windowManager) {
            _queryModel = queryModel;
            _commandInvoker = commandInvoker;
            _guidGenerator = guidGenerator;
            _areaViewModelFactory = areaViewModelFactory;
            _windowManager = windowManager;

            _queryModel.ModelHasChanged += (s, e) => RefreshView();
        }

        public void AddLanguage() {
            var vm = new AddLanguageViewModel();
            OpenDataView(vm,
                         c => new AddLanguageCommand(_guidGenerator.Next(),
                                                     c.IsoName,
                                                     c.LanguageDisplayName));
        }

        public void SetLanguage(Language language) {
            if(language == null)
                return;

            var vm = new AddLanguageViewModel();
            vm.LanguageDisplayName = language.DisplayName;
            vm.IsoName = language.IsoName;

            var dialogResult = _windowManager.ShowDialog(vm);
            if (dialogResult.HasValue && dialogResult.Value)
                _commandInvoker.Invoke(new SetLanguageCommand(language.Id,
                                                              vm.IsoName,
                                                              vm.LanguageDisplayName));
        }

        public void RemoveLanguage(Language language) {
            if(language == null) return;

            _commandInvoker.Invoke(new RemoveLanguageCommand(language.Id));
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
            var vm = new AddLocalizedTextViewModel(_queryModel,
                                                   _areaViewModelFactory);
            OpenDataView(vm,
                         c => new AddLocalizedTextCommand(c.AreaId,
                                                          c.KeyId,
                                                          _guidGenerator.Next(),
                                                          c.LanguageId,
                                                          c.Text));
        }

        private void OpenDataView<T>(T vm,
                                     Func<T, BaseCommand> createCommand) where T : Screen {
            var result = _windowManager.ShowDialog(vm);
            if (result.HasValue && result.Value) {
                _commandInvoker.Invoke(createCommand(vm));
            }
        }

        private void RefreshView() {
            NotifyOfPropertyChange(() => Languages);
            NotifyOfPropertyChange(() => Areas);
            NotifyOfPropertyChange(() => Keys);
            NotifyOfPropertyChange(() => Texts);
        }
    }

    public interface IWorkbench {}
}