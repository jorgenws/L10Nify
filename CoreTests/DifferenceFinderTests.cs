using System;
using System.Collections.Generic;
using Core;
using Moq;
using NUnit.Framework;

namespace CoreTests {
    [TestFixture]
    public class DifferenceFinderTests {
        private Mock<ILoadedLocalization> _loadedLocalization;
        private Mock<ILocalization> _localization;

        private readonly Guid _languageId = Guid.Parse("{0520C019-B49D-4866-9BD6-D87751EE304B}");

        [SetUp]
        public void SetUp() {
            _loadedLocalization = new Mock<ILoadedLocalization>();
            _localization = new Mock<ILocalization>();
        }

        [Test]
        public void LanguageDifference_AddedOneLanguage_CorrectText() {
            _loadedLocalization.Setup(c => c.Languages)
                               .Returns(new List<Language>());
            _localization.Setup(c => c.Languages())
                         .Returns(new List<Language> {
                                                         CreateLanguage()
                                                     });

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.LanguageChanges(_loadedLocalization.Object,
                                                             _localization.Object);

            Assert.IsTrue(result.Contains("1 language(s) have been added."));
        }

        [Test]
        public void LanguageDifference_RemovedOneLanguage_CorrectText() {
            _loadedLocalization.Setup(c => c.Languages)
                .Returns(new List<Language> {
                                                         CreateLanguage()
                                                     });
                               
            _localization.Setup(c => c.Languages())
                         .Returns(new List<Language>());

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.LanguageChanges(_loadedLocalization.Object,
                                                             _localization.Object);

            Assert.IsTrue(result.Contains("1 language(s) have been removed."));
        }

        [Test]
        public void LanguageDifference_ChangedOneLanguage_CorrectText() {
            var previousLanguage = CreateLanguage();
            _loadedLocalization.Setup(c => c.Languages)
                               .Returns(new List<Language> {
                                                               previousLanguage
                                                           });
            var currentLanguage = CreateLanguage();
            currentLanguage.DisplayName = "test";
            _localization.Setup(c => c.Languages())
                         .Returns(new List<Language> {
                                                         currentLanguage
                                                     });

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.LanguageChanges(_loadedLocalization.Object,
                                                             _localization.Object);

            Assert.IsTrue(result.Contains("1 language(s) have been changed."));
        }



        private Language CreateLanguage() {
            return new Language {
                                    Id = _languageId
                                };
        }
    }
}
