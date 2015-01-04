using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Localization : ILocalization {
        private readonly Dictionary<Guid,Language> _languages;
        private readonly List<HistoryEntry> _historyEntries;
        private readonly Dictionary<Guid,Area> _area;
        private readonly Dictionary<Guid, LocalizationKey> _keys;
        private readonly Dictionary<Guid, LocalizedText> _texts;

        public Localization() {
            _languages = new Dictionary<Guid, Language>();
            _area = new Dictionary<Guid, Area>();
            _keys = new Dictionary<Guid, LocalizationKey>();
            _texts = new Dictionary<Guid, LocalizedText>();

            _historyEntries = new List<HistoryEntry>();
        }

        public void AddArea(Area area) {
            if (_area.ContainsKey(area.Id) || _area.Values.Any(c => c.Name == area.Name))
                throw new Exception("Cannot add more then one area with the same name");

            _area.Add(area.Id,
                      area);
        }

        public void SetArea(Guid areaId,
                            string newName,
                            string newComment,
                            byte[] newImage) {
            if (!_area.ContainsKey(areaId))
                throw new Exception("Area does not exist");

            _area[areaId].Name = newName;
            _area[areaId].Comment = newComment;
            _area[areaId].Image = newImage;
        }

        public void RemoveArea(Guid areaId) {
            if (_area.ContainsKey(areaId)) {
                _area.Remove(areaId);
                
                //removed everything related with this area
                var keysToRemove = _keys.Values.Where(c => c.AreaId == areaId)
                                        .ToDictionary(c => c.Id);
                var textsToRemove = _texts.Values.Where(c => keysToRemove.ContainsKey(c.KeyId))
                                          .ToList();

                foreach (var key in keysToRemove.Values)
                    _keys.Remove(key.Id);

                foreach (var text in textsToRemove)
                    _texts.Remove(text.Id);
            }
        }

        public void AddLocalizationKey(LocalizationKey key) {
            if (!_area.ContainsKey(key.AreaId))
                throw new Exception("Cannot find area");

            if (_keys.ContainsKey(key.Id) || _keys.Values.Any(c => c.Key == key.Key))
                throw new Exception("Cannot add more the one key with the same name");

            _keys.Add(key.Id, key);
        }

        public void ChangeKeyName(Guid keyId,
                                  string newKeyName) {
            if (!_keys.ContainsKey(keyId))
                throw new Exception("Key does not exist");

            _keys[keyId].Key = newKeyName;
        }

        public void RemoveLocalizationKey(Guid keyId) {
            if (_keys.ContainsKey(keyId)) {
                _keys.Remove(keyId);

                var textToBeRemoved = _texts.Values.Where(c => c.KeyId == keyId)
                                            .ToList();

                foreach (var text in textToBeRemoved)
                    _texts.Remove(text.Id);
            }
                
        }

        public void AddLocalizedText(Guid areaId,
                                     LocalizedText text) {
            if (!_area.ContainsKey(areaId))
                throw new Exception("Cannot find area");

            if (!_keys.ContainsKey(text.KeyId))
                throw new Exception("Cannot find key");

            if (_texts.ContainsKey(text.Id) || _texts.Values.Any(c => c.LanguageId == text.LanguageId && c.KeyId == text.KeyId))
                throw new Exception("Cannot add the same language twice");

            _texts.Add(text.Id,
                       text);
        }

        public void ChangeText(Guid textId,
                                    string newText) {
            if (!_texts.ContainsKey(textId))
                throw new Exception("Text does not exist");

            _texts[textId].Text = newText;
        }

        public void RemoveLocalizedText(Guid localizedTextId) {
            if (_texts.ContainsKey(localizedTextId))
                _texts.Remove(localizedTextId);
        }

        public void AddLanguage(Language language) {
            if (_languages.ContainsKey(language.Id) || _languages.Values.Any(c => c.IsoName == language.IsoName))
                throw new Exception("Cannot add more then one language with the same id");

            _languages.Add(language.Id, language);
        }

        public void ChangeLanguageDisplayName(Guid languageId,
                                              string newDisplayName) {
            if (!_languages.ContainsKey(languageId))
                throw new Exception("Language does not exist");

            _languages[languageId].DisplayName = newDisplayName;
        }

        public void RemoveLanguage(Guid languageId) {
            if (_languages.ContainsKey(languageId)) {
                _languages.Remove(languageId);

                //remove all texts associated with the language
                var textsToBeRemoved = _texts.Values.Where(c => c.LanguageId == languageId)
                                             .ToList();
                foreach (var text in textsToBeRemoved)
                    _texts.Remove(text.Id);
            }
        }

        public void AddHistoryEntry(HistoryEntry entry) {
            _historyEntries.Add(entry);
        }

        public IEnumerable<Area> RetriveAreas() {
            return _area.Values;
        }

        public Area RetriveArea(Guid areaId) {
            if (_area.ContainsKey(areaId))
                return _area[areaId];

            return null;
        }

        public IEnumerable<LocalizationKey> RetriveKeys() {
            return _keys.Values;
        }

        public LocalizationKey RetriveKey(Guid keyId) {
            if (_keys.ContainsKey(keyId))
                return _keys[keyId];

            return null;
        }

        public IEnumerable<LocalizedText> RetriveTexts() {
            return _texts.Values;
        }

        public LocalizedText RetiveText(Guid textId) {
            if (_texts.ContainsKey(textId)) 
                return _texts[textId];

            return null;
        }

        public IEnumerable<Language> RetriveLanguages() {
            return _languages.Values;
        }

        public Language RetriveLanguage(Guid languageId) {
            if (_languages.ContainsKey(languageId))
                return _languages[languageId];

            return null;
        }

        public IEnumerable<HistoryEntry> RetriveHistory() {
            return _historyEntries;
        }

        public IEnumerable<MissingLocalizedText> RetriveMissingLocalizedTexts() {
            return from key in RetriveKeys()
                   from language in RetriveLanguages()
                   where !RetriveTexts()
                              .Any(c => c.KeyId == key.Id && c.LanguageId == language.Id)
                   select new MissingLocalizedText(key.AreaId,
                                                   key.Id,
                                                   language.Id);
        }
    }

    public interface ILocalization {
        void AddArea(Area area);
        void SetArea(Guid areaId,
                     string newName,
                     string newComment,
                     byte[] newImage);
        void RemoveArea(Guid areaId);
        void AddLocalizationKey(LocalizationKey key);
        void ChangeKeyName(Guid keyId,
                           string newKeyName);
        void RemoveLocalizationKey(Guid itemId);
        void AddLocalizedText(Guid areaId,
                              LocalizedText text);
        void ChangeText(Guid textId,
                             string newText);
        void RemoveLocalizedText(Guid localizedTextId);
        void AddLanguage(Language language);
        void ChangeLanguageDisplayName(Guid languageId,
                                       string newDisplayName);
        void RemoveLanguage(Guid languageId);
        void AddHistoryEntry(HistoryEntry entry);
        IEnumerable<Area> RetriveAreas();
        Area RetriveArea(Guid areaId);
        IEnumerable<LocalizationKey> RetriveKeys();
        LocalizationKey RetriveKey(Guid keyId);
        IEnumerable<LocalizedText> RetriveTexts();
        LocalizedText RetiveText(Guid textId);
        IEnumerable<Language> RetriveLanguages();
        Language RetriveLanguage(Guid languageId);
        IEnumerable<HistoryEntry> RetriveHistory();
        IEnumerable<MissingLocalizedText> RetriveMissingLocalizedTexts();
    }

    public class Area {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public byte[] Image { get; set; }
    }

    public class LocalizationKey {
        public Guid Id { get; set; }
        public Guid AreaId { get; set; }
        public string Key { get; set; }
    }

    public class LocalizedText {
        public Guid Id { get; set; }
        public Guid KeyId { get; set; }
        public string Text { get; set; }
        public Guid LanguageId { get; set; }
    }

    public class MissingLocalizedText {
        public Guid AreaId { get; private set; }
        public Guid KeyId { get; private set; }
        public Guid LanguageId { get; private set; }

        public MissingLocalizedText(Guid areaId,
                                    Guid keyId,
                                    Guid languageId) {
            AreaId = areaId;
            KeyId = keyId;
            LanguageId = languageId;
        }
    }
}