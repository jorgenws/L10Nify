using System;
using System.Linq;

namespace Core {
    public class LocalizationBuilder : ILocalizationBuilder {
        public ILocalization Build(ILoadedLocalization loadedLocalization) {
            if (loadedLocalization == null)
                throw new Exception("Cannot build from a null object");

            var localization = new Localization();

            foreach (var area in loadedLocalization.Areas) {
                var copy = CopyArea(area);
                localization.AddArea(copy);
            }

            foreach (var key in loadedLocalization.Keys) {
                var copy = CopyKey(key);
                localization.AddLocalizationKey(copy);
            }

            foreach (var text in loadedLocalization.Texts) {
                var copy = CopyText(text);
                var key = localization.RetriveKey(copy.KeyId);
                localization.AddLocalizedText(key.AreaId,
                                              copy);
            }

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

        public ILoadedLocalization Build(ILocalization localization, string filePath) {
            if (localization == null)
                throw new Exception("Cannot build from a null object");

            var loadedLocalization = new LoadedLocalization(filePath,
                                                            localization.RetriveAreas()
                                                                        .Select(CopyArea)
                                                                        .ToList(),
                                                            localization.RetriveKeys()
                                                                        .Select(CopyKey)
                                                                        .ToList(),
                                                            localization.RetriveTexts()
                                                                        .Select(CopyText)
                                                                        .ToList(),
                                                            localization.RetriveLanguages()
                                                                        .Select(CopyLanguage)
                                                                        .ToList(),
                                                            localization.RetriveHistory()
                                                                        .Select(CopyHistoryEntry)
                                                                        .ToList());
            return loadedLocalization;
        }

        private LocalizedText CopyText(LocalizedText text) {
            return new LocalizedText {
                                         Id = text.Id,
                                         Text = text.Text,
                                         LanguageId = text.LanguageId,
                                         KeyId = text.KeyId
                                     };
        }

        private LocalizationKey CopyKey(LocalizationKey key) {
            return new LocalizationKey {
                                           Id = key.Id,
                                           Key = key.Key,
                                           AreaId = key.AreaId
                                       };
        }

        private Area CopyArea(Area area) {
            return new Area {
                                Id = area.Id,
                                Name = area.Name,
                                Comment = area.Comment,
                                Image = area.Image
                            };
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
        ILoadedLocalization Build(ILocalization localization, string filePath);
    }
}