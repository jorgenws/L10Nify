using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Model : IQueryModel {
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

        public void AddArea(Guid areaId, string name, string comment, byte[] image) {
            var area = _areaFactory.Create(areaId,
                                           name,
                                           comment,
                                           image);
            _localization.AddArea(area);
        }

        public void SetArea(Guid areaId,
                            string newName,
                            string newComment,
                            byte[] newImage) {
            _localization.SetArea(areaId,
                                  newName,
                                  newComment,
                                  newImage);
        }

        public void RemoveArea(Guid areaId) {
            _localization.RemoveArea(areaId);
        }

        public void AddLocalizationKey(Guid areaId,
                                       Guid keyId,
                                       string key) {
            var localizationKey = _localizationKeyFactory.Create(areaId,
                                                                 keyId,
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
                                     Guid textId,
                                     Guid languageId,
                                     string text) {
            var localizedText = _localizedTextFactory.Create(textId,
                                                             keyId,
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

        public void AddLanguage(Guid languageId,
                                string isoName,
                                string displayName) {
            var language = _languageFactory.Create(languageId,
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

        public Area RetriveArea(Guid areaId) {
            return _localization.RetriveArea(areaId);
        }

        public IEnumerable<LocalizationKey> RetriveLocalizationKeys() {
            return _localization.RetriveKeys();
        }

        public LocalizationKey RetriveLocalizationKey(Guid localizationKeyId) {
            return _localization.RetriveKey(localizationKeyId);
        }

        public IEnumerable<LocalizedText> RetriveLocalizedTexts() {
            return _localization.RetriveTexts();
        }

        public LocalizedText RetriveLocalizedText(Guid localizedTextId) {
            return _localization.RetiveText(localizedTextId);
        }

        public IEnumerable<Language> RetriveLanguages() {
            return _localization.RetriveLanguages();
        }

        public Language RetriveLanguage(Guid languageId) {
            return _localization.RetriveLanguage(languageId);
        }

        public IEnumerable<HistoryEntry> RetriveHistoryEntries() {
            return _localization.RetriveHistory();
        }

        public bool HasFileName() {
            if (_loadedLocalization == null) return false;
            if (string.IsNullOrWhiteSpace(_loadedLocalization.FilePath)) return false;

            return true;
        }

        public void Save() {
            var historyEntry = _historyEntryFactory.Create(_localization,
                                                           _loadedLocalization);
            _localization.AddHistoryEntry(historyEntry);

            _localizationPersister.Write(_loadedLocalization.FilePath,
                                         _localization);

            MoveCurrentToLoadedWhenSaving(_loadedLocalization.FilePath);
        }

        public void SaveAs(string filePath) {
            var historyEntry = _historyEntryFactory.Create(_localization,
                                                           _loadedLocalization);

            _localization.AddHistoryEntry(historyEntry);

            _localizationPersister.Write(filePath,
                                         _localization);

            MoveCurrentToLoadedWhenSaving(filePath);
        }

        private void MoveCurrentToLoadedWhenSaving(string filePath) {
            _loadedLocalization = _localizationBuilder.Build(_localization,
                                                             filePath);
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

    public interface IQueryModel {
        IEnumerable<Area> RetriveAreas();
        Area RetriveArea(Guid areaId);
        IEnumerable<LocalizationKey> RetriveLocalizationKeys();
        LocalizationKey RetriveLocalizationKey(Guid localizationKeyId);
        IEnumerable<LocalizedText> RetriveLocalizedTexts();
        LocalizedText RetriveLocalizedText(Guid localizedTextId);
        IEnumerable<Language> RetriveLanguages();
        Language RetriveLanguage(Guid languageId);
        IEnumerable<HistoryEntry> RetriveHistoryEntries();

        bool HasFileName();
    }
}