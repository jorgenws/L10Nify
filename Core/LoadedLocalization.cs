using System.Collections.Generic;

namespace Core {
    public class LoadedLocalization : ILoadedLocalization {
        public string FileName { get; private set; }
        public List<Language> Languages { get; private set; }
        public List<HistoryEntry> HistoryEntries { get; private set; }

        public LoadedLocalization(string fileName,
                                  List<Language> languages,
                                  List<HistoryEntry> historyEntries) {
            FileName = fileName;
            Languages = languages;
            HistoryEntries = historyEntries;
        }
    }

    public interface ILoadedLocalization {
        string FileName { get; }
        List<Language> Languages { get; }
        List<HistoryEntry> HistoryEntries { get; }
    }
}