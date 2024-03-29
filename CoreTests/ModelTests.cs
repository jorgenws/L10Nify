﻿using System;
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
        private Mock<ILocalizationVisitorFactory> _visitorFactory;

        private Mock<ILocalization> _localization;

        private readonly Guid _languageId = Guid.Parse("{B031E3C4-6143-4C5E-9E7B-DC427B9E96EB}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";
        private const int LCID = 1;

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
            _visitorFactory = new Mock<ILocalizationVisitorFactory>();
        }

        [Test]
        public void AddLanguage_AddingLanguage_LanguageIsAdded() {
            var language = CreateDefaultLanguage();

            _languageFactory.Setup(c => c.Create(_languageId,
                                                 IsoName,
                                                 LCID,
                                                 DisplayName))
                            .Returns(language);

            var model = CreateDefaultModel();
            model.AddLanguage(_languageId,
                              IsoName,
                              LCID,
                              DisplayName);

            _localization.Verify(c => c.AddLanguage(language));
        }

        [Test]
        public void ChangeLanguageDisplayName_IsOk_ChangedToLocalization() {
            const string newDisplayName = "displayName";
            const string newLanguageRegion = "bl";
            const int newLcid = 2;
            var model = CreateDefaultModel();
            model.SetLanguage(_languageId,
                              newLanguageRegion,
                              newLcid,
                              newDisplayName);
            _localization.Verify(c => c.SetLanguage(_languageId,
                                                    newLanguageRegion,
                                                    newLcid,
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
            _areaFactory.Setup(c => c.Create(_areaId,
                                             areaName,
                                             null,
                                             null))
                        .Returns(area);

            var model = CreateDefaultModel();

            model.AddArea(_areaId,
                          areaName,
                          null,
                          null);

            _localization.Verify(c => c.AddArea(area));
        }

        [Test]
        public void ChangeArea_IsOk_ChangedToLocalization() {
            const string areaName = "name";
            const string areaComment = "comment";
            byte[] areaImage = new byte[0];
            var model = CreateDefaultModel();

            model.SetArea(_areaId,
                          areaName,
                          areaComment,
                          areaImage);

            _localization.Verify(c => c.SetArea(_areaId,
                                                areaName,
                                                areaComment,
                                                areaImage));
        }

        [Test]
        public void AddLocalizationKey_IsOk_AddedToLocalization() {
            const string localizationKeyName = "name";
            var key = CreateDefaultLocalizationKey();
            _localizationKeyFactory.Setup(c => c.Create(_areaId,
                                                        _keyId,
                                                        localizationKeyName))
                                   .Returns(key);

            var model = CreateDefaultModel();

            model.AddLocalizationKey(_areaId,
                                     _keyId,
                                     localizationKeyName);

            _localization.Verify(c => c.AddLocalizationKey(key));
        }

        [Test]
        public void ChangeLocalizationKey_IsOk_ChangedToLocalization() {
            const string areaName = "name";
            var model = CreateDefaultModel();

            model.SetLocalizationKey(_keyId,
                                     _areaId,
                                     areaName);

            _localization.Verify(c => c.ChangeKey(_keyId,
                                                  _areaId,
                                                  areaName));
        }

        [Test]
        public void AddLocalizedText_IsOk_AddedToLocalization() {
            const string text = "name";
            var localizedText = CreateDefaultLocalizedText();
            _localizedTextFactory.Setup(c => c.Create(_textId,
                                                      _keyId,
                                                      _languageId,
                                                      text))
                                 .Returns(localizedText);

            var model = CreateDefaultModel();

            model.AddLocalizedText(_areaId,
                                   _keyId,
                                   _textId,
                                   _languageId,
                                   text);

            _localization.Verify(c => c.AddLocalizedText(_areaId,
                                                         localizedText));
        }

        [Test]
        public void ChangeLocalizedText_IsOk_ChangedToLocalization() {
            const string text = "name";
            var model = CreateDefaultModel();

            model.SetLocalizedText(_areaId,
                                   _keyId,
                                   _textId,
                                   _languageId,
                                   text);

            _localization.Verify(c => c.SetText(_areaId,
                                                _keyId,
                                                _textId,
                                                _languageId,
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

        [Test]
        public void HasFileName_NotLoadedLocalization_IsFalse() {
            var model = CreateDefaultModel();
            
            var result = model.HasFileName();

            Assert.IsFalse(result);
        }

        private Model CreateDefaultModel() {
            return new Model(_languageFactory.Object,
                             _historyEntryFactory.Object,
                             _areaFactory.Object,
                             _localizationKeyFactory.Object,
                             _localizedTextFactory.Object,
                             _persister.Object,
                             _loader.Object,
                             _builder.Object,
                             _visitorFactory.Object);
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