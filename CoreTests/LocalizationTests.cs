using System;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LocalizationTests {
        private readonly Guid _languageId = Guid.Parse("{46E0090F-61A0-43FB-A782-AD07337F92A3}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";
        
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
            var languageFactory = CreateLocalization();

            languageFactory.RemoveLanguage(_languageId);

            CollectionAssert.IsEmpty(languageFactory.Languages());
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
    }
}
