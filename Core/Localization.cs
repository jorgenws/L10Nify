using System;
using System.Collections.Generic;
using System.Linq;

namespace Core {
    public class Localization : ILocalization {
        private readonly List<Language> _languages;
        private readonly List<HistoryEntry> _historyEntries;
        private readonly List<Area> _area;

        public Localization() {
            _languages = new List<Language>();
            _historyEntries = new List<HistoryEntry>();
            _area = new List<Area>();
        }

        public void AddArea(Area area) {
            if (_area.Any(c => c.Name == area.Name))
                throw new Exception("Cannot add more then one area with the same name");

                _area.Add(area);
        }

        public void RemoveArea(Guid areaId) {
            var area = _area.SingleOrDefault(c => c.Id == areaId);
            if (area != null)
                _area.Remove(area);
        }

        public void AddLocalizedItem(Guid areaId,
                                     LocalizedItem item) {
            var area = _area.SingleOrDefault(c => c.Id == areaId);
            if (area == null)
                throw new Exception("Cannot find area");

            if (area.Items.Any(c => c.Id == item.Id || c.Key == item.Key))
                throw new Exception("Cannot add more the one key with the same name");

            area.Items.Add(item);
        }

        public void RemoveLocalizedItem(Guid areaId,
                                        Guid itemId) {
            var area = _area.SingleOrDefault(c => c.Id == areaId);
            if(area == null) return;

            var item = area.Items.SingleOrDefault(c => c.Id == itemId);
            if(item == null) return;

            area.Items.Remove(item);
        }

        public void AddLocalizedText(Guid areaId,
                                     Guid itemId,
                                     LocalizedText text) {
            var area = _area.SingleOrDefault(c => c.Id == areaId);
            if (area == null)
                throw new Exception("Cannot find area");

            var item = area.Items.SingleOrDefault(c => c.Id == itemId);
            if (item == null)
                throw new Exception("Cannot find item");

            if (item.Texts.Any(c => c.Id == text.Id || c.LanguageId == text.LanguageId))
                throw new Exception("Cannot add the same language twice");

            item.Texts.Add(text);

        }

        public void RemoveLocalizedText(Guid areaId,
                                        Guid itemId,
                                        Guid localizedTextId) {
            var area = _area.SingleOrDefault(c => c.Id == areaId);
            if(area == null) return;

            var item = area.Items.SingleOrDefault(c => c.Id == itemId);
            if (item == null) return;

            var text = item.Texts.SingleOrDefault(c => c.Id == localizedTextId);
            if (text == null) return;

            item.Texts.Remove(text);
        }

        public List<Area> Areas() {
            return _area;
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
        void AddArea(Area area);
        void RemoveArea(Guid areaId);
        void AddLocalizedItem(Guid areaId,
                              LocalizedItem item);
        void RemoveLocalizedItem(Guid areaId,
                                 Guid itemId);
        void AddLocalizedText(Guid areaId,
                              Guid itemId,
                              LocalizedText text);
        void RemoveLocalizedText(Guid areaId,
                                 Guid itemId,
                                 Guid localizedTextId);
        void AddLanguage(Language language);
        void RemoveLanguage(Guid id);
        List<Language> Languages();
        void AddHistoryEntry(HistoryEntry entry);
        List<HistoryEntry> History();
    }

    public class Area {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<LocalizedItem> Items { get; set; }
    }

    public class LocalizedItem {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public List<LocalizedText> Texts { get; set; }
    }

    public class LocalizedText {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid LanguageId { get; set; }
    }
}