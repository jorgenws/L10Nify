using System;
using System.Collections.Generic;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class ModelTests {
        private Mock<ILanguageFactory> _languageFactory;
        private Mock<IHistoryEntryFactory> _historyEntryFactory;
        private Mock<ILocalizationPersister> _persister;
        private Mock<ILocalizationLoader> _loader;
        private Mock<ILocalizationBuilder> _builder;

        private Mock<ILocalization> _localization;

        private readonly Guid _languageId = Guid.Parse("{B031E3C4-6143-4C5E-9E7B-DC427B9E96EB}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";

        [SetUp]
        public void SetUp() {
            _languageFactory = new Mock<ILanguageFactory>();
            _historyEntryFactory = new Mock<IHistoryEntryFactory>();
            _persister = new Mock<ILocalizationPersister>();
            _loader = new Mock<ILocalizationLoader>();
            _localization = new Mock<ILocalization>();
            _builder = new Mock<ILocalizationBuilder>();
            _builder.Setup(c => c.Build(It.IsAny<ILoadedLocalization>()))
                    .Returns(_localization.Object);
        }

        [Test]
        public void AddLanguage_AddingLanguage_LanguageIsAdded() {
            var language = CreateLanguage();

            _languageFactory.Setup(c => c.Create(_languageId,
                                                 IsoName,
                                                 DisplayName))
                            .Returns(language);

            var model = CreateDefaultModel();
            model.AddLanguage(_languageId,
                              IsoName,
                              DisplayName);

            _localization.Verify(c => c.AddLanguage(language));
        }

        [Test]
        public void RemoveLanguage_RemovingLanguage_LanguageIsRemoved() {
           var model = CreateDefaultModel();
            model.RemoveLanguage(_languageId);

            _localization.Verify(c => c.RemoveLanguage(_languageId));
        }

        [Test]
        public void RetriveLanguages_OneLanguage_ReturnsOneLanguage() {
            var language = CreateLanguage();
            _localization.Setup(c => c.Languages())
                         .Returns(new List<Language> {
                                                         language
                                                     });

            var model = CreateDefaultModel();

            var result = model.RetriveLanguages();

            CollectionAssert.Contains(result,
                                      language);
        }

        [Test]
        public void RetriveHistoryEntries_OneHistoryEntry_ReturnsOneHistoryEntry() {
            var historyEntry = CreateHistoryEntry();
            _localization.Setup(c => c.History())
                         .Returns(new List<HistoryEntry> {
                                                             historyEntry
                                                         });

            var model = CreateDefaultModel();

            var result = model.RetriveHistoryEntries();

            CollectionAssert.Contains(result,
                                      historyEntry);
        }

        private Model CreateDefaultModel() {
            return new Model(_languageFactory.Object,
                             _historyEntryFactory.Object,
                             _persister.Object,
                             _loader.Object,
                             _builder.Object);
        }

        private Language CreateLanguage() {
            return new Language();
        }

        private HistoryEntry CreateHistoryEntry() {
            return new HistoryEntry(DateTime.Now,
                                    string.Empty);
        }
    }
}
