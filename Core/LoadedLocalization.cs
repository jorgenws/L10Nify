using System.Collections.Generic;

namespace Core {
    public class LoadedLocalization : ILoadedLocalization {
        public string FileName { get; private set; }
        public List<Area> Areas { get; private set; }
        public List<LocalizationKey> Keys { get; private set; }
        public List<LocalizedText> Texts { get; private set; }
        public List<Language> Languages { get; private set; }
        public List<HistoryEntry> HistoryEntries { get; private set; }

        public LoadedLocalization(string fileName,
                                  List<Area> areas,
                                  List<LocalizationKey> keys,
                                  List<LocalizedText> texts,
                                  List<Language> languages,
                                  List<HistoryEntry> historyEntries) {
            FileName = fileName;
            Languages = languages;
            HistoryEntries = historyEntries;
            Areas = areas;
            Texts = texts;
            Keys = keys;
        }
    }

    public interface ILoadedLocalization {
        string FileName { get; }
        List<Area> Areas { get; }
        List<LocalizationKey> Keys { get; }
        List<LocalizedText> Texts { get; }
        List<Language> Languages { get; }
        List<HistoryEntry> HistoryEntries { get; }
    }
}