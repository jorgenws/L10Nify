﻿using System;

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
                localization.AddLocalizedKey(copy);
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

        private LocalizedText CopyText(LocalizedText text) {
            return new LocalizedText {
                                         Id = text.Id,
                                         Value = text.Value,
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
                                Name = area.Name
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
    }
}