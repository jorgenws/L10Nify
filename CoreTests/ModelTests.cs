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
        private Mock<IAreaFactory> _areaFactory;
        private Mock<ILocalizationKeyFactory> _localizationKeyFactory;
        private Mock<ILocalizedTextFactory> _localizedTextFactory;
        private Mock<ILocalizationPersister> _persister;
        private Mock<ILocalizationLoader> _loader;
        private Mock<ILocalizationBuilder> _builder;

        private Mock<ILocalization> _localization;

        private readonly Guid _languageId = Guid.Parse("{B031E3C4-6143-4C5E-9E7B-DC427B9E96EB}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";

        private readonly Guid _areaId = Guid.Parse("{357122ED-F1E4-4B8A-85E2-187DEEFB333F}");
        private readonly Guid _keyId = Guid.Parse("{BBE95265-4769-4C7D-B0BE-EAA1B92E8F6C}");
        private readonly Guid _textId = Guid.Parse("{CF648E27-8854-4687-B263-EDEA70EC1314}");

        [SetUp]
        public void SetUp() {
            _languageFactory = new Mock<ILanguageFactory>();
            _historyEntryFactory = new Mock<IHistoryEntryFactory>();
            _areaFactory = new Mock<IAreaFactory>();
            _localizationKeyFactory = new Mock<ILocalizationKeyFactory>();
            _localizedTextFactory = new Mock<ILocalizedTextFactory>();
            _persister = new Mock<ILocalizationPersister>();
            _loader = new Mock<ILocalizationLoader>();
            _localization = new Mock<ILocalization>();
            _builder = new Mock<ILocalizationBuilder>();
            _builder.Setup(c => c.Build(It.IsAny<ILoadedLocalization>()))
                    .Returns(_localization.Object);
        }

        [Test]
        public void AddLanguage_AddingLanguage_LanguageIsAdded() {
            var language = CreateDefaultLanguage();

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
        public void ChangeLanguageDisplayName_IsOk_ChangedToLocalization() {
            const string newDisplayName = "displayName";
            var model = CreateDefaultModel();
            model.ChangeLanguageDisplayName(_languageId,
                                            newDisplayName);
            _localization.Setup(c => c.ChangeLanguageDisplayName(_languageId,
                                                                 newDisplayName));
        }

        [Test]
        public void RemoveLanguage_RemovingLanguage_LanguageIsRemoved() {
           var model = CreateDefaultModel();
            model.RemoveLanguage(_languageId);

            _localization.Verify(c => c.RemoveLanguage(_languageId));
        }

        [Test]
        public void RetriveLanguages_OneLanguage_ReturnsOneLanguage() {
            var language = CreateDefaultLanguage();
            _localization.Setup(c => c.RetriveLanguages())
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
            var historyEntry = CreateDefaultHistoryEntry();
            _localization.Setup(c => c.RetriveHistory())
                         .Returns(new List<HistoryEntry> {
                                                             historyEntry
                                                         });

            var model = CreateDefaultModel();

            var result = model.RetriveHistoryEntries();

            CollectionAssert.Contains(result,
                                      historyEntry);
        }

        [Test]
        public void AddArea_IsOk_AddedToLocalization() {
            const string areaName = "name";
            var area = CreateDefaultArea();
            _areaFactory.Setup(c => c.Create(areaName))
                        .Returns(area);

            var model = CreateDefaultModel();

            model.AddArea(areaName);

            _localization.Verify(c => c.AddArea(area));
        }

        [Test]
        public void ChangeArea_IsOk_ChangedToLocalization() {
            const string areaName = "name";
            var model = CreateDefaultModel();

            model.ChangeAreaName(_areaId,
                                 areaName);

            _localization.Verify(c => c.ChangeAreaName(_areaId,
                                                       areaName));
        }

        [Test]
        public void AddLocalizationKey_IsOk_AddedToLocalization() {
            const string localizationKeyName = "name";
            var key = CreateDefaultLocalizationKey();
            _localizationKeyFactory.Setup(c => c.Create(_areaId,
                                                        localizationKeyName))
                                   .Returns(key);

            var model = CreateDefaultModel();

            model.AddLocalizationKey(_areaId, localizationKeyName);

            _localization.Verify(c => c.AddLocalizationKey(key));
        }

        [Test]
        public void ChangeLocalizationKey_IsOk_ChangedToLocalization() {
            const string areaName = "name";
            var model = CreateDefaultModel();

            model.ChangeLocalizationKeyName(_areaId,
                                            areaName);

            _localization.Verify(c => c.ChangeKeyName(_areaId,
                                                      areaName));
        }

        [Test]
        public void AddLocalizedText_IsOk_AddedToLocalization() {
            const string text = "name";
            var localizedText = CreateDefaultLocalizedText();
            _localizedTextFactory.Setup(c => c.Create(_keyId,
                                                      _languageId,
                                                      text))
                                 .Returns(localizedText);

            var model = CreateDefaultModel();

            model.AddLocalizedText(_areaId,
                                   _keyId,
                                   _languageId,
                                   text);

            _localization.Verify(c => c.AddLocalizedText(_areaId,
                                                         localizedText));
        }

        [Test]
        public void ChangeLocalizedText_IsOk_ChangedToLocalization() {
            const string text = "name";
            var model = CreateDefaultModel();

            model.ChangeLocalizedText(_areaId,
                                      text);

            _localization.Verify(c => c.ChangeText(_areaId,
                                                   text));
        }

        [Test]
        public void RemoveArea_IsOk_RemovedFromLocalization() {
            var model = CreateDefaultModel();
            model.RemoveArea(_areaId);
            _localization.Verify(c => c.RemoveArea(_areaId));
        }

        [Test]
        public void RemoveLocalizationKey_IsOk_RemovedFromLocalization() {
            var model = CreateDefaultModel();
            model.RemoveLocalizationKey(_keyId);
            _localization.Verify(c => c.RemoveLocalizationKey(_keyId));
        }

        [Test]
        public void RemoveLocalizedText_IsOk_RemovedFromLocalization() {
            var model = CreateDefaultModel();
            model.RemoveLocalizedText(_textId);
            _localization.Verify(c => c.RemoveLocalizedText(_textId));
        }

        private Model CreateDefaultModel() {
            return new Model(_languageFactory.Object,
                             _historyEntryFactory.Object,
                             _areaFactory.Object,
                             _localizationKeyFactory.Object,
                             _localizedTextFactory.Object,
                             _persister.Object,
                             _loader.Object,
                             _builder.Object);
        }

        private Language CreateDefaultLanguage() {
            return new Language();
        }

        private HistoryEntry CreateDefaultHistoryEntry() {
            return new HistoryEntry(DateTime.Now,
                                    string.Empty);
        }

        private Area CreateDefaultArea() {
            return new Area();
        }

        private LocalizationKey CreateDefaultLocalizationKey() {
            return new LocalizationKey();
        }

        private LocalizedText CreateDefaultLocalizedText() {
            return new LocalizedText();
        }
    }
}
