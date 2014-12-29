using System;
using System.Linq;
using Core;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LocalizationTests {
        private readonly Guid _languageId = Guid.Parse("{46E0090F-61A0-43FB-A782-AD07337F92A3}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";

        private readonly Guid _areaId = Guid.Parse("{DF227C31-D5A0-4B33-A0E9-F5E528ABACBB}");
        private const string AreaName = "Area";

        private readonly Guid _keyId = Guid.Parse("{AF469E82-2000-446F-8F13-9D02D594D883}");
        private const string ItemKey = "Key";

        private readonly Guid _textId = Guid.Parse("{D5EEDD4C-D52E-4F02-B4A0-D4E00FBBE3AD}");
        private const string Text = "Text";

        
        [Test]
        public void AddLanguage_LanguageIsOk_IsAdded() {
            var language = CreateDefaultLanguage();

            var localization = CreateLocalization();

            localization.AddLanguage(language);

            CollectionAssert.Contains(localization.Languages(),
                                      language);
        }

        [Test]
        public void AddLanguage_LanguageIdIsAlreadyInUse_ThrowsException() {
            var language = CreateDefaultLanguage();

            var localization = CreateLocalization();

            localization.AddLanguage(language);

            Assert.Throws<Exception>(() => localization.AddLanguage(language));
        }

        [Test]
        public void RemoveLanguage_LanguageExists_IsRemoved() {
            var language = CreateDefaultLanguage();

            var localization = CreateLocalization();

            localization.AddLanguage(language);

            localization.RemoveLanguage(_languageId);

            CollectionAssert.DoesNotContain(localization.Languages(),
                                            language);
        }

        [Test]
        public void RemoveLanguage_LanguageDoesNotExist_NotingChanges() {
            var localization = CreateLocalization();

            localization.RemoveLanguage(_languageId);

            CollectionAssert.IsEmpty(localization.Languages());
        }

        [Test]
        public void AddArea_NewArea_AreaIsAdded() {
            var area = CreateDefaultArea();
            var localization = CreateLocalization();

            localization.AddArea(area);

            CollectionAssert.Contains(localization.Areas(), area);
        }

        [Test]
        public void AddArea_DuplicateArea_AreaIsAdded() {
            var area = CreateDefaultArea();
            var localization = CreateLocalization();

            localization.AddArea(area);
            
            Assert.Throws<Exception>(() => localization.AddArea(area));
        }

        [Test]
        public void RemoveArea_ThereIsOneArea_RemovedTheArea() {
            var area = CreateDefaultArea();
            var localization = CreateLocalization();

            localization.AddArea(area);
            localization.RemoveArea(area.Id);

            CollectionAssert.DoesNotContain(localization.Areas(),
                                            area);
        }

        [Test]
        public void AddLocalizedItem_NoMatchingArea_ThrowsException() {
            var localizationKey = CreateDefaultCLocalizedItem();
            var localization = CreateLocalization();

            Assert.Throws<Exception>(() => localization.AddLocalizedKey(localizationKey));
        }

        [Test]
        public void AddLocalizedItem_HasArea_IsAdded() {
            var area = CreateDefaultArea();
            var localizationKey = CreateDefaultCLocalizedItem();

            var localization = CreateLocalization();
            localization.AddArea(area);
            localization.AddLocalizedKey(localizationKey);
            CollectionAssert.Contains(localization.Keys(), localizationKey);
        }

        [Test]
        public void RemoveLocalizedItem_ItemExists_IsRemoved() {
            var area = CreateDefaultArea();
            var localizedItem = CreateDefaultCLocalizedItem();

            var localization = CreateLocalization();
            localization.AddArea(area);
            localization.AddLocalizedKey(localizedItem);
            localization.RemoveLocalizationKey(localizedItem.Id);
            CollectionAssert.DoesNotContain(localization.Keys(),
                                            localizedItem);
        }

        [Test]
        public void AddLocalizedText_AreaDoesNotExits_ThrowsException() {
            var localizedText = CreateDefaultLocalizedText();

            var localization = CreateLocalization();

            Assert.Throws<Exception>(() => localization.AddLocalizedText(_areaId,
                                                                         localizedText));
        }

        [Test]
        public void AddLocalizedText_ItemDoesNotExist_ThrowsException() {
            var area = CreateDefaultArea();
            var localizedText = CreateDefaultLocalizedText();

            var localization = CreateLocalization();
            localization.AddArea(area);

            Assert.Throws<Exception>(() => localization.AddLocalizedText(_areaId,
                                                                         localizedText));
        }

        [Test]
        public void AddLocalizedText_IsOk_IsAdded() {
            var area = CreateDefaultArea();
            var key = CreateDefaultCLocalizedItem();
            var text = CreateDefaultLocalizedText();

            var localization = CreateLocalization();

            localization.AddArea(area);
            localization.AddLocalizedKey(key);
            localization.AddLocalizedText(area.Id,
                                          text);

            CollectionAssert.Contains(localization.Texts(),
                                      text);
        }

        [Test]
        public void RemoveLocalizationText_RemoveAText_IsRemoved() {
            var area = CreateDefaultArea();
            var item = CreateDefaultCLocalizedItem();
            var text = CreateDefaultLocalizedText();

            var localization = CreateLocalization();

            localization.AddArea(area);
            localization.AddLocalizedKey(item);
            localization.AddLocalizedText(area.Id,
                                          text);
            localization.RemoveLocalizedText(text.Id);

            CollectionAssert.DoesNotContain(localization.Texts(),
                                            text);
        }

        private Localization CreateLocalization() {
            return new Localization();
        }

        private Language CreateDefaultLanguage() {
            return new Language {
                                    Id = _languageId,
                                    IsoName = IsoName,
                                    DisplayName = DisplayName
                                };
        }

        private Area CreateDefaultArea() {
            return new Area {
                                Id = _areaId,
                                Name = AreaName
                            };
        }

        private LocalizationKey CreateDefaultCLocalizedItem() {
            return new LocalizationKey {
                                         Id = _keyId,
                                         Key = ItemKey,
                                         AreaId = _areaId
                                     };
        }

        private LocalizedText CreateDefaultLocalizedText() {
            return new LocalizedText {
                                         Id = _textId,
                                         KeyId = _keyId,
                                         LanguageId = _languageId,
                                         Value = Text
                                     };
        }
    }
}
