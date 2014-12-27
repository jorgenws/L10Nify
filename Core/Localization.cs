using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Localization {
        private readonly ILanguageFactory _languageFactory;
        private readonly List<Language> _languages;

        public Localization(ILanguageFactory languageFactory) {
            _languages = new List<Language>();

            _languageFactory = languageFactory;
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
    }
}