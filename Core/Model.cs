using System;
using System.Collections.Generic;

namespace Core {
    public class Model {
        private ILoadedLocalization _loadedLocalization;
        private ILocalization _localization;

        private readonly ILanguageFactory _languageFactory;
        private readonly IHistoryEntryFactory _historyEntryFactory;
        private readonly ILocalizationPersister _localizationPersister;
        private readonly ILocalizationLoader _localizationLoader;
        private readonly ILocalizationBuilder _localizationBuilder;

        public Model(ILanguageFactory languageFactory,
                     IHistoryEntryFactory historyEntryFactory,
                     ILocalizationPersister localizationPersister,
                     ILocalizationLoader localizationLoader,
                     ILocalizationBuilder localizationBuilder) {
            _languageFactory = languageFactory;
            _historyEntryFactory = historyEntryFactory;
            _localizationPersister = localizationPersister;
            _localizationLoader = localizationLoader;
            _localizationBuilder = localizationBuilder;

            New();
        }

        public void AddArea(string name) {
            
        }

        

        public void AddLanguage(Guid id,
                                string isoName,
                                string displayName) {
            var language = _languageFactory.Create(id,
                                                   isoName,
                                                   displayName);
            _localization.AddLanguage(language);
        }

        public void RemoveLanguage(Guid id) {
            _localization.RemoveLanguage(id);
        }

        public IEnumerable<Language> RetriveLanguages() {
            return _localization.Languages();
        }

        public IEnumerable<HistoryEntry> RetriveHistoryEntries() {
            return _localization.History();
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
                                                         new List<Language>(),
                                                         new List<HistoryEntry>());
            _localization = _localizationBuilder.Build(_loadedLocalization);
        }
    }
}