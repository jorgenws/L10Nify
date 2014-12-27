﻿using System;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class LocalizationTests {
        private readonly Guid _languageId = Guid.Parse("{46E0090F-61A0-43FB-A782-AD07337F92A3}");
        private const string IsoName = "no";
        private const string DisplayName = "Norsk";
        
        private Mock<ILanguageFactory> _languageFactory;

        [SetUp]
        public void SetUp() {
            _languageFactory = new Mock<ILanguageFactory>();
        }

        [Test]
        public void AddLanguage_LanguageIsOk_IsAdded() {
            var language = CreateDefaultLanguage();
            _languageFactory.Setup(c => c.Create(_languageId,
                                                 IsoName,
                                                 DisplayName))
                            .Returns(language);

            var localization = CreateLocalization(_languageFactory.Object);

            localization.AddALanguage(_languageId,
                                      IsoName,
                                      DisplayName);

            CollectionAssert.Contains(localization.Languages(),
                                      language);
        }

        [Test]
        public void AddLanguage_LanguageIdIsAlreadyInUse_ThrowsException() {
            var language = CreateDefaultLanguage();
            _languageFactory.Setup(c => c.Create(_languageId,
                                                 IsoName,
                                                 DisplayName))
                            .Returns(language);

            var localization = CreateLocalization(_languageFactory.Object);

            localization.AddALanguage(_languageId,
                                      IsoName,
                                      DisplayName);

            Assert.Throws<Exception>(() => localization.AddALanguage(_languageId,
                                                                     IsoName,
                                                                     DisplayName));
        }

        [Test]
        public void RemoveLanguage_LanguageExists_IsRemoved() {
            var language = CreateDefaultLanguage();
            _languageFactory.Setup(c => c.Create(_languageId,
                                                 IsoName,
                                                 DisplayName))
                            .Returns(language);

            var localization = CreateLocalization(_languageFactory.Object);

            localization.AddALanguage(_languageId,
                                      IsoName,
                                      DisplayName);

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

        private Localization CreateLocalization(ILanguageFactory languageFactory) {
            return new Localization(languageFactory);
        }

        private Localization CreateLocalization() {
            return new Localization(_languageFactory.Object);
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