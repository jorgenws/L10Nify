using System;

namespace Core {
    public class LocalizationBuilder : ILocalizationBuilder {
        public ILocalization Build(ILoadedLocalization loadedLocalization) {
            if (loadedLocalization == null)
                throw new Exception("Cannot build from a null object");

            var localization = new Localization();

            foreach (Language language in loadedLocalization.Languages) {
                var copy = CopyLanguage(language);
                localization.AddLanguage(copy);
            }

            foreach (HistoryEntry entry in loadedLocalization.HistoryEntries) {
                var copy = CopyHistoryEntry(entry);
                localization.AddHistoryEntry(copy);
            }

            return localization;
        }

        private Language CopyLanguage(Language language) {
            return new Language {
                                    Id = language.Id,
                                    IsoName = language.IsoName,
                                    DisplayName = language.DisplayName
                                };
        }

        private HistoryEntry CopyHistoryEntry(HistoryEntry entry) {
            return new HistoryEntry(entry.SaveDate,
                                    entry.Changes);
        }
    }

    public interface ILocalizationBuilder {
        ILocalization Build(ILoadedLocalization loadedLocalization);
    }
}