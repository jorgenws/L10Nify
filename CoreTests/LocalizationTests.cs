﻿using System;
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

            CollectionAssert.Contains(localization.RetriveLanguages(),
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

            CollectionAssert.DoesNotContain(localization.RetriveLanguages(),
                                            language);
        }

        [Test]
        public void RemoveLanguage_LanguageDoesNotExist_NotingChanges() {
            var localization = CreateLocalization();

            localization.RemoveLanguage(_languageId);

            CollectionAssert.IsEmpty(localization.RetriveLanguages());
        }

        [Test]
        public void AddArea_NewArea_AreaIsAdded() {
            var area = CreateDefaultArea();
            var localization = CreateLocalization();

            localization.AddArea(area);

            CollectionAssert.Contains(localization.RetriveAreas(), area);
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

            CollectionAssert.DoesNotContain(localization.RetriveAreas(),
                                            area);
        }

        [Test]
        public void AddLocalizedItem_NoMatchingArea_ThrowsException() {
            var localizationKey = CreateDefaultLocalizationKey();
            var localization = CreateLocalization();

            Assert.Throws<Exception>(() => localization.AddLocalizationKey(localizationKey));
        }

        [Test]
        public void AddLocalizedItem_HasArea_IsAdded() {
            var area = CreateDefaultArea();
            var localizationKey = CreateDefaultLocalizationKey();

            var localization = CreateLocalization();
            localization.AddArea(area);
            localization.AddLocalizationKey(localizationKey);
            CollectionAssert.Contains(localization.RetriveKeys(), localizationKey);
        }

        [Test]
        public void RemoveLocalizedItem_ItemExists_IsRemoved() {
            var area = CreateDefaultArea();
            var localizedItem = CreateDefaultLocalizationKey();

            var localization = CreateLocalization();
            localization.AddArea(area);
            localization.AddLocalizationKey(localizedItem);
            localization.RemoveLocalizationKey(localizedItem.Id);
            CollectionAssert.DoesNotContain(localization.RetriveKeys(),
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
            var key = CreateDefaultLocalizationKey();
            var text = CreateDefaultLocalizedText();

            var localization = CreateLocalization();

            localization.AddArea(area);
            localization.AddLocalizationKey(key);
            localization.AddLocalizedText(area.Id,
                                          text);

            CollectionAssert.Contains(localization.RetriveTexts(),
                                      text);
        }

        [Test]
        public void RemoveLocalizationText_RemoveAText_IsRemoved() {
            var area = CreateDefaultArea();
            var item = CreateDefaultLocalizationKey();
            var text = CreateDefaultLocalizedText();

            var localization = CreateLocalization();

            localization.AddArea(area);
            localization.AddLocalizationKey(item);
            localization.AddLocalizedText(area.Id,
                                          text);
            localization.RemoveLocalizedText(text.Id);

            CollectionAssert.DoesNotContain(localization.RetriveTexts(),
                                            text);
        }

        [Test]
        public void ChangeAreaName_AreaDoesNotExist_ThrowsException() {
            var localization = CreateLocalization();
            Assert.Throws<Exception>(() => localization.ChangeAreaName(_areaId,
                                                                       "test"));
        }

        [Test]
        public void ChangeAreaName_AreExists_AreaNameIsChanged() {
            const string newAreaName = "area51";
            var area = CreateDefaultArea();

            var localization = CreateLocalization();
            localization.AddArea(area);

            localization.ChangeAreaName(area.Id,
                                        newAreaName);

            Assert.AreEqual(newAreaName,
                            area.Name);
        }

        [Test]
        public void ChangeKeyName_KeyDoesNotExist_ThrowsException() {
            var localization = CreateLocalization();
            Assert.Throws<Exception>(() => localization.ChangeKeyName(_areaId,
                                                                      "test"));
        }

        [Test]
        public void ChangeKeyName_KeyExist_KeyIdChanged() {
            const string newKey = "skeleton key";
            var area = CreateDefaultArea();
            var key = CreateDefaultLocalizationKey();

            var localization = CreateLocalization();
            localization.AddArea(area);
            localization.AddLocalizationKey(key);

            localization.ChangeKeyName(key.Id,
                                       newKey);
            Assert.AreEqual(newKey,
                            key.Key);
        }

        [Test]
        public void ChangeText_TextDoesNotExist_ThrowsException() {
            var localization = CreateLocalization();
            Assert.Throws<Exception>(() => localization.ChangeText(_textId,
                                                                   "test"));
        }

        [Test]
        public void ChangeText_TextExists_TextIsChanged() {
            const string newText = "new text";
            var area = CreateDefaultArea();
            var key = CreateDefaultLocalizationKey();
            var text = CreateDefaultLocalizedText();

            var localization = CreateLocalization();
            localization.AddArea(area);
            localization.AddLocalizationKey(key);
            localization.AddLocalizedText(area.Id,
                                          text);

            localization.ChangeText(text.Id,
                                    newText);

            Assert.AreEqual(newText,
                            text.Value);
        }

        [Test]
        public void ChangeLanguageDisplayName_LanguageDoesNotExist_ThrowsException() {
            var localization = CreateLocalization();
            Assert.Throws<Exception>(() => localization.ChangeLanguageDisplayName(_languageId,
                                                                                  "test"));
        }

        [Test]
        public void ChangeLanguageDisplayName_LanguageExists_LanguageDisplayNameChanged() {
            const string newDisplayName = "newDisplayName";
            var language = CreateDefaultLanguage();
            var localization = CreateLocalization();

            localization.AddLanguage(language);

            localization.ChangeLanguageDisplayName(language.Id,
                                                   newDisplayName);

            Assert.AreEqual(newDisplayName,
                            language.DisplayName);
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

        private LocalizationKey CreateDefaultLocalizationKey() {
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
