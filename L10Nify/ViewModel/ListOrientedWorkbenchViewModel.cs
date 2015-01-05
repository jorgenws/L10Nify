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

        public IEnumerable<LocalizationKey> Keys {
            get {
                return _queryModel.RetriveLocalizationKeys()
                                  .ToList();
            }
        }

        public IEnumerable<LocalizedText> Texts {
            get {
                return _queryModel.RetriveLocalizedTexts()
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
                                                     c.LanguageRegion,
                                                     c.LCID,
                                                     c.LanguageDisplayName));
        }

        public void SetLanguage(Language language) {
            if(language == null)
                return;

            var vm = new AddLanguageViewModel();
            vm.SetCultureInfo(language.LCID);

            var dialogResult = _windowManager.ShowDialog(vm);
            if (dialogResult.HasValue && dialogResult.Value)
                _commandInvoker.Invoke(new SetLanguageCommand(language.Id,
                                                              vm.LanguageRegion,
                                                              vm.LCID,
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

        public void SetArea(AreaViewModel areaViewModel) {
            if (areaViewModel == null) return;

            var vm = new AddAreaViewModel();
            vm.AreaName = areaViewModel.Name;
            vm.Comment = areaViewModel.Comment;
            vm.Image = areaViewModel.ImageSource;
            var dialogresult = _windowManager.ShowDialog(vm);
            if (dialogresult.HasValue && dialogresult.Value)
                _commandInvoker.Invoke(new SetAreaCommand(areaViewModel.Id,
                                                          vm.AreaName,
                                                          vm.Comment,
                                                          vm.Image));
        }

        public void RemoveArea(AreaViewModel areaViewModel) {
            if (areaViewModel == null) return;

            _commandInvoker.Invoke(new RemoveAreaCommand(areaViewModel.Id));
        }

        public void AddKey() {
            var vm = new AddLocalizationKeyViewModel(_queryModel);
            OpenDataView(vm,
                         c => new AddLocalizationKeyCommand(c.AreaId,
                                                            _guidGenerator.Next(),
                                                            c.KeyName));
        }

        public void SetKey(LocalizationKey key) {
            if (key == null) return;

            var vm = new AddLocalizationKeyViewModel(_queryModel);
            vm.AreaId = key.AreaId;
            vm.KeyName = key.Key;
            var dialogresult = _windowManager.ShowDialog(vm);
            if(dialogresult.HasValue && dialogresult.Value)
                _commandInvoker.Invoke(new SetLocalizationKeyNameCommand(key.Id, vm.AreaId, vm.KeyName));
        }

        public void RemoveKey(LocalizationKey key) {
            if (key == null) return;

            _commandInvoker.Invoke(new RemoveLocalizationKeyCommand(key.Id));
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

        public void SetLocalizedText(LocalizedText text) {
            if (text == null) return;

            var key = _queryModel.RetriveLocalizationKey(text.KeyId);

            var vm = new AddLocalizedTextViewModel(_queryModel,
                                                   _areaViewModelFactory);
            vm.KeyId = text.KeyId;
            vm.LanguageId = text.LanguageId;
            vm.AreaId = key.AreaId;
            vm.Text = text.Text;
            var dialogresult = _windowManager.ShowDialog(vm);
            if (dialogresult.HasValue && dialogresult.Value)
                _commandInvoker.Invoke(new SetLocalizedTextCommand(text.Id,
                                                                   vm.KeyId,
                                                                   vm.AreaId,
                                                                   vm.LanguageId,
                                                                   vm.Text));
        }

        public void RemoveLocalizedText(LocalizedText text) {
            if (text == null) return;

            _commandInvoker.Invoke(new RemoveLocalizedTextCommand(text.Id));
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