using System;
using System.Collections.Generic;

namespace Core {
    public class Model {
        private ILoadedLocalization _loadedLocalization;
        private ILocalization _localization;

        private readonly ILanguageFactory _languageFactory;
        private readonly IHistoryEntryFactory _historyEntryFactory;
        private readonly IAreaFactory _areaFactory;
        private readonly ILocalizationKeyFactory _localizationKeyFactory;
        private readonly ILocalizedTextFactory _localizedTextFactory;
        private readonly ILocalizationPersister _localizationPersister;
        private readonly ILocalizationLoader _localizationLoader;
        private readonly ILocalizationBuilder _localizationBuilder;

        public Model(ILanguageFactory languageFactory,
                     IHistoryEntryFactory historyEntryFactory,
                     IAreaFactory areaFactory,
                     ILocalizationKeyFactory localizationKeyFactory,
                     ILocalizedTextFactory localizedTextFactory,
                     ILocalizationPersister localizationPersister,
                     ILocalizationLoader localizationLoader,
                     ILocalizationBuilder localizationBuilder) {
            _languageFactory = languageFactory;
            _historyEntryFactory = historyEntryFactory;
            _areaFactory = areaFactory;
            _localizationKeyFactory = localizationKeyFactory;
            _localizedTextFactory = localizedTextFactory;
            _localizationPersister = localizationPersister;
            _localizationLoader = localizationLoader;
            _localizationBuilder = localizationBuilder;

            New();
        }

        public void AddArea(string name) {
            var area = _areaFactory.Create(name);
            _localization.AddArea(area);
        }

        public void ChangeAreaName(Guid areaId,
                                   string newName) {
            _localization.ChangeAreaName(areaId,
                                         newName);
        }

        public void RemoveArea(Guid areaId) {
            _localization.RemoveArea(areaId);
        }

        public void AddLocalizationKey(Guid areaId,
                                       string key) {
            var localizationKey = _localizationKeyFactory.Create(areaId,
                                                                 key);
            _localization.AddLocalizationKey(localizationKey);
        }

        public void ChangeLocalizationKeyName(Guid localizationKeyId,
                                              string newKey) {
            _localization.ChangeKeyName(localizationKeyId,
                                        newKey);
        }

        public void RemoveLocalizationKey(Guid keyId) {
            _localization.RemoveLocalizationKey(keyId);
        }

        public void AddLocalizedText(Guid areaId,
                                     Guid keyId,
                                     Guid languageId,
                                     string text) {
            var localizedText = _localizedTextFactory.Create(keyId,
                                                             languageId,
                                                             text);
            _localization.AddLocalizedText(areaId,
                                           localizedText);
        }

        public void ChangeLocalizedText(Guid textId,
                                        string newText) {
            _localization.ChangeText(textId,
                                     newText);
        }

        public void RemoveLocalizedText(Guid textId) {
            _localization.RemoveLocalizedText(textId);
        }

        public void AddLanguage(Guid id,
                                string isoName,
                                string displayName) {
            var language = _languageFactory.Create(id,
                                                   isoName,
                                                   displayName);
            _localization.AddLanguage(language);
        }

        public void ChangeLanguageDisplayName(Guid languageId,
                                              string newDisplayName) {
            _localization.ChangeLanguageDisplayName(languageId,
                                                    newDisplayName);
        }

        public void RemoveLanguage(Guid id) {
            _localization.RemoveLanguage(id);
        }

        public IEnumerable<Area> RetriveAreas() {
            return _localization.RetriveAreas();
        }

        public IEnumerable<LocalizationKey> RetriveLocalizationKeys() {
            return _localization.RetriveKeys();
        }

        public IEnumerable<LocalizedText> RetriveLocalizedTexts() {
            return _localization.RetriveTexts();
        }

        public IEnumerable<Language> RetriveLanguages() {
            return _localization.RetriveLanguages();
        }

        public IEnumerable<HistoryEntry> RetriveHistoryEntries() {
            return _localization.RetriveHistory();
        }

        public void Save() {
            var historyEntry = _historyEntryFactory.Create(_localization,
                                                           _loadedLocalization);
            _localization.AddHistoryEntry(historyEntry);

            _localizationPersister.Write(_loadedLocalization.FileName,
                                         _localization);
        }

        public void Load(string filePath) {
            _loadedLocalization = _localizationLoader.Load(filePath);
            _localization = _localizationBuilder.Build(_loadedLocalization);
        }

        public void New() {
            _loadedLocalization = new LoadedLocalization(string.Empty,
                                                         new List<Area>(),
                                                         new List<LocalizationKey>(),
                                                         new List<LocalizedText>(),
                                                         new List<Language>(),
                                                         new List<HistoryEntry>());
            _localization = _localizationBuilder.Build(_loadedLocalization);
        }
    }
}