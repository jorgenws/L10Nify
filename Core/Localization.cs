using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Localization {
        private readonly ILanguageFactory _languageFactory;
        private readonly IHistoryEntryFactory _historyEntryFactory;
        private readonly IPersister _persister;
        private readonly List<Language> _languages;
        private readonly List<HistoryEntry> _historyEntries;

        private Localization _loadedLocalization; //Create separeate readonly class

        public Localization(ILanguageFactory languageFactory,
            IHistoryEntryFactory historyEntryFactory,
            IPersister persister) {
            _languages = new List<Language>();
            _historyEntries = new List<HistoryEntry>();

            _languageFactory = languageFactory;
            _historyEntryFactory = historyEntryFactory;
            _persister = persister;
        }

        public void AddALanguage(Guid id,
                                 string isoName,
                                 string displayName) {
            if (_languages.Any(c => c.Id == id))
                throw new Exception("Cannot add more then one language with the same id");

            var language = _languageFactory.Create(id,
                                                   isoName,
                                                   displayName);
            _languages.Add(language);
        }

        public void RemoveLanguage(Guid id) {
            //ToDo: Add check if the removed language is in use. If in use then it will not be allowed to be removed.
            
            var language = _languages.SingleOrDefault(c => c.Id == id);
            if (language != null)
                _languages.Remove(language);
        }

        public List<Language> Languages() {
            return _languages;
        }

        public void Save() {
            var historyEntry = _historyEntryFactory.Create(_languages,
                                                           _loadedLocalization);
            _historyEntries.Add(historyEntry);

            _persister.Write("some file name, default to the existing one",
                             this);
        }

        public List<HistoryEntry> History() {
            return _historyEntries;
        }
    }
}