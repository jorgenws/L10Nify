using System;
using System.Collections.Generic;

namespace Core {
    public class HistoryEntryFactory : IHistoryEntryFactory {
        public HistoryEntry Create(List<Language> languages,
                                   Localization localization) {
            return localization == null
                       ? new HistoryEntry(DateTime.Now,
                                          "Localization file created")
                       : new HistoryEntry(DateTime.Now,
                                          "Figure out changes between this version and the previous one");
        }
    }

    public interface IHistoryEntryFactory {
        HistoryEntry Create(List<Language> languages,
                            Localization localization);
    }
}