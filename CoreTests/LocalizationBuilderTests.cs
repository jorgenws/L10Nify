using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LocalizationBuilderTests {
        private readonly DateTime _date = DateTime.Now;
        private const string Comment = "test";

        private readonly Guid _languageId = Guid.Parse("{34F0AD5E-B947-4F8D-BAFC-6EF24148611B}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";

        [Test]
        public void Build_IsNull_ThrowsException() {
            var builder = new LocalizationBuilder();

            Assert.Throws<Exception>(() => builder.Build(null));
        }

        [Test]
        public void Build_HasOneLanguageAndOneHistoryEntry_CreatesACopy() {
            var language = new Language {
                                            Id = _languageId,
                                            LanguageRegion = IsoName,
                                            DisplayName = DisplayName
                                        };
            var historyEntry = new HistoryEntry(_date,
                                                Comment);

            var loadedLocalization = CreateLoadedLocalization(new List<Area>(),
                                                              new List<LocalizationKey>(),
                                                              new List<LocalizedText>(),
                                                              new List<Language> {
                                                                                     language
                                                                                 },
                                                              new List<HistoryEntry> {
                                                                                         historyEntry
                                                                                     });

            var builder = new LocalizationBuilder();
            var localization = builder.Build(loadedLocalization);

            var copiedLanguage = localization.RetriveLanguages()
                                             .First();
            var copiedHistoryEntry = localization.RetriveHistory()
                                                 .First();

            Assert.AreNotSame(language,
                              copiedLanguage);
            Assert.AreEqual(copiedLanguage.Id,
                            language.Id);
            Assert.AreEqual(copiedLanguage.LanguageRegion,
                            language.LanguageRegion);
            Assert.AreEqual(copiedLanguage.DisplayName,
                            language.DisplayName);
            Assert.AreNotSame(historyEntry,
                              copiedHistoryEntry);
            Assert.AreEqual(copiedHistoryEntry.SaveDate,
                            historyEntry.SaveDate);
            Assert.AreEqual(copiedHistoryEntry.Changes,
                            historyEntry.Changes);

        }

        private ILoadedLocalization CreateLoadedLocalization(List<Area> areas,
                                                             List<LocalizationKey> keys,
                                                             List<LocalizedText> texts,
                                                             List<Language> languages,
                                                             List<HistoryEntry> entries) {
            return new LoadedLocalization(string.Empty,
                                          areas,
                                          keys,
                                          texts,
                                          languages,
                                          entries);
        }

    }
}