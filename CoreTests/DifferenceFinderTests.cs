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
        
        private readonly Guid _areaId = Guid.Parse("{971714A7-6C96-499F-AFEA-2544EABD30DE}");

        [SetUp]
        public void SetUp() {
            _loadedLocalization = new Mock<ILoadedLocalization>();
            _loadedLocalization.Setup(c => c.Areas)
                               .Returns(new List<Area>());
            _loadedLocalization.Setup(c => c.Keys)
                               .Returns(new List<LocalizationKey>());
            _loadedLocalization.Setup(c => c.Texts)
                               .Returns(new List<LocalizedText>());
            _loadedLocalization.Setup(c => c.Languages)
                               .Returns(new List<Language>());
            _localization = new Mock<ILocalization>();
            _localization.Setup(c => c.RetriveAreas())
                         .Returns(new List<Area>());
            _localization.Setup(c => c.RetriveKeys())
                         .Returns(new List<LocalizationKey>());
            _localization.Setup(c => c.RetriveTexts())
                         .Returns(new List<LocalizedText>());
            _localization.Setup(c => c.RetriveLanguages())
                         .Returns(new List<Language>());
        }

        [Test]
        public void Changes_AddedOneLanguage_CorrectText() {
            _loadedLocalization.Setup(c => c.Languages)
                               .Returns(new List<Language>());
            _localization.Setup(c => c.RetriveLanguages())
                         .Returns(new List<Language> {
                                                         CreateLanguage()
                                                     });

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.Changes(_loadedLocalization.Object,
                                                             _localization.Object);

            Assert.IsTrue(result.Contains("1 language(s) have been added."));
        }

        [Test]
        public void Changes_RemovedOneLanguage_CorrectText() {
            _loadedLocalization.Setup(c => c.Languages)
                .Returns(new List<Language> {
                                                         CreateLanguage()
                                                     });
                               
            _localization.Setup(c => c.RetriveLanguages())
                         .Returns(new List<Language>());

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.Changes(_loadedLocalization.Object,
                                                             _localization.Object);

            Assert.IsTrue(result.Contains("1 language(s) have been removed."));
        }

        [Test]
        public void Changes_ChangedOneLanguage_CorrectText() {
            var previousLanguage = CreateLanguage();
            _loadedLocalization.Setup(c => c.Languages)
                               .Returns(new List<Language> {
                                                               previousLanguage
                                                           });
            var currentLanguage = CreateLanguage();
            currentLanguage.DisplayName = "test";
            _localization.Setup(c => c.RetriveLanguages())
                         .Returns(new List<Language> {
                                                         currentLanguage
                                                     });

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.Changes(_loadedLocalization.Object,
                                                             _localization.Object);

            Assert.IsTrue(result.Contains("1 language(s) have been changed."));
        }

        [Test]
        public void Changes_AddedOneArea_CorrectText() {
            _loadedLocalization.Setup(c => c.Areas)
                               .Returns(new List<Area>());
            _localization.Setup(c => c.RetriveAreas())
                         .Returns(new List<Area> {
                                                     CreateArea()
                                                 });

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.Changes(_loadedLocalization.Object,
                                                     _localization.Object);

            Assert.IsTrue(result.Contains("1 area(s) have been added."));
        }

        [Test]
        public void Changes_RemovedOneArea_CorrectText() {
            _loadedLocalization.Setup(c => c.Areas)
                               .Returns(new List<Area> {
                                                           CreateArea()
                                                       });
            _localization.Setup(c => c.RetriveAreas())
                         .Returns(new List<Area>());

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.Changes(_loadedLocalization.Object,
                                                     _localization.Object);

            Assert.IsTrue(result.Contains("1 area(s) have been removed."));
        }

        [Test]
        public void Changes_ChangedOneArea_CorrectText() {
            var previous = CreateArea();
            var current = CreateArea();
            current.Name = "test";

            _loadedLocalization.Setup(c => c.Areas)
                               .Returns(new List<Area> {
                                                           previous
                                                       });
            _localization.Setup(c => c.RetriveAreas())
                         .Returns(new List<Area> {
                                                     current
                                                 });

            var differenceFinder = new DifferenceFinder();

            string result = differenceFinder.Changes(_loadedLocalization.Object,
                                                     _localization.Object);

            Assert.IsTrue(result.Contains("1 area(s) have been changed."));
        }

        private Language CreateLanguage() {
            return new Language {
                                    Id = _languageId
                                };
        }

        private Area CreateArea() {
            return new Area {
                                Id = _areaId
                            };
        }
    }
}
