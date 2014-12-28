using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Localization : ILocalization {
        private readonly List<Language> _languages;
        private readonly List<HistoryEntry> _historyEntries;

        public Localization() {
            _languages = new List<Language>();
            _historyEntries = new List<HistoryEntry>();
        }

        public void AddLanguage(Language language) {
            if (_languages.Any(c => c.Id == language.Id))
                throw new Exception("Cannot add more then one language with the same id");

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

        public void AddHistoryEntry(HistoryEntry entry) {
            _historyEntries.Add(entry);
        }

        public List<HistoryEntry> History() {
            return _historyEntries;
        }
    }

    public interface ILocalization {
        void AddLanguage(Language language);
        void RemoveLanguage(Guid id);
        List<Language> Languages();
        void AddHistoryEntry(HistoryEntry entry);
        List<HistoryEntry> History();
    }
}