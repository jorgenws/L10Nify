using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using L10Nify;
using Moq;
using NUnit.Framework;

namespace L10NifyTests {
    [TestFixture]
    public class TreeViewModelBuilderTests {
        private Mock<IQueryModel> _queryModel;
        private IAreaViewModelFactory _areaViewModelFactory;

        private readonly Guid _areaId = Guid.Parse("{C7BA6498-9CC2-45B3-86B1-8A19995EC769}");
        private readonly Guid _keyId = Guid.Parse("{885F34AF-11C5-4902-9A20-23435A9A921F}");
        private readonly Guid _languageId = Guid.Parse("{C4E63176-9E9E-445A-86ED-635B942E61A9}");
        private readonly Guid _language2Id = Guid.Parse("{1534FA40-59EC-499C-9A4F-901B6E902716}");
        private readonly Guid _textId = Guid.Parse("{9C34D957-7A20-410B-A301-8ED406C4E6DC}");
        private readonly Guid _text2Id = Guid.Parse("{693AA19E-CBF7-49AB-9730-2DD6EFD4588D}");

        [SetUp]
        public void SetUp() {
            _queryModel = new Mock<IQueryModel>();
            _areaViewModelFactory = new AreaViewModelFactory();
        }

        [Test]
        public void Build_HasOneArea_ReturnsTreeWithOneArea() {
            _queryModel.Setup(c => c.RetriveAreas())
                       .Returns(new[] {
                                          CreateDefaultArea()
                                      });

            var builder = CreateDefaultTreeViewModelBuilder();

            IEnumerable<ITreeNodeViewModel> result = builder.Build(_queryModel.Object);

            Assert.IsInstanceOf<AreaTreeNodeViewModel>(result.First());
            var areaVM = result.First() as AreaTreeNodeViewModel;
            Assert.AreEqual(_areaId,
                            areaVM.Id);


        }

        [Test]
        public void Build_HasOneAreaAndOneKey_ReturnsTreeWithOneAreaAndOneKey() {
            _queryModel.Setup(c => c.RetriveAreas())
                       .Returns(new[] {
                                          CreateDefaultArea()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizationKeys())
                       .Returns(new[] {
                                          CreateDefaultLocalizationKey()
                                      });

            var builder = CreateDefaultTreeViewModelBuilder();

            var result = builder.Build(_queryModel.Object);

            var areaVM = result.First() as AreaTreeNodeViewModel;

            Assert.IsInstanceOf<LocalizationKeyTreeNodeViewModel>(areaVM.Children.First());
            var keyVM = areaVM.Children.First() as LocalizationKeyTreeNodeViewModel;
            Assert.AreEqual(_keyId,
                            keyVM.Id);
        }

        [Test]
        public void Build_HasOneAreaOneKeyAndOneLocalizedText_ReturnsTreeWithOneAreaAndOneKeyAndOneLocalizedText() {
            _queryModel.Setup(c => c.RetriveAreas())
                       .Returns(new[] {
                                          CreateDefaultArea()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizationKeys())
                       .Returns(new[] {
                                          CreateDefaultLocalizationKey()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizedTexts())
                       .Returns(new[] {
                                          CreateDefaultLocalizedText()
                                      });
            _queryModel.Setup(c => c.RetriveLanguage(_languageId))
                       .Returns(CreateDefaultLanguage());

            var builder = CreateDefaultTreeViewModelBuilder();

            var result = builder.Build(_queryModel.Object);

            var areaVM = result.First() as AreaTreeNodeViewModel;
            var keyVM = areaVM.Children.First() as LocalizationKeyTreeNodeViewModel;
            Assert.IsInstanceOf<LocalizedTextTreeNodeViewModel>(keyVM.Children.First());
            var localizedTextVM = keyVM.Children.First() as LocalizedTextTreeNodeViewModel;
            Assert.AreEqual(_textId,
                            localizedTextVM.Id);
        }

        [Test]
        public void Build_HasOneAreaOneKeyTwoLanguagesAndTexts_ReturnsTreeWithOneAreaAndOneKeyTwoLanguagesAndTexts() {
            _queryModel.Setup(c => c.RetriveAreas())
           .Returns(new[] {
                                          CreateDefaultArea()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizationKeys())
                       .Returns(new[] {
                                          CreateDefaultLocalizationKey()
                                      });
            
            var language = CreateDefaultLanguage();
            var language2 = CreateDefaultLanguage();
            language2.Id = _language2Id;
            _queryModel.Setup(c => c.RetriveLanguage(_languageId))
                       .Returns(language);
            _queryModel.Setup(c => c.RetriveLanguage(_language2Id))
                       .Returns(language2);

            var text = CreateDefaultLocalizedText();
            var text2 = CreateDefaultLocalizedText();
            text2.Id = _text2Id;
            text2.LanguageId = _language2Id;
            _queryModel.Setup(c => c.RetriveLocalizedTexts())
                       .Returns(new[] {
                                          text,
                                          text2
                                      });

            var builder = CreateDefaultTreeViewModelBuilder();

            var result = builder.Build(_queryModel.Object);

            var areaVM = result.First() as AreaTreeNodeViewModel;
            var keyVM = areaVM.Children.First() as LocalizationKeyTreeNodeViewModel;
            Assert.AreEqual(2,
                            keyVM.Children.Count());
        }

        private TreeViewModelBuilder CreateDefaultTreeViewModelBuilder() {
            return new TreeViewModelBuilder(_areaViewModelFactory);
        }

        private Area CreateDefaultArea() {
            return new Area {
                                Id = _areaId
                            };
        }

        private LocalizationKey CreateDefaultLocalizationKey() {
            return new LocalizationKey {
                                           Id = _keyId,
                                           AreaId = _areaId
                                       };
        }

        private Language CreateDefaultLanguage() {
            return new Language {
                                    Id = _languageId
                                };
        }

        private LocalizedText CreateDefaultLocalizedText() {
            return new LocalizedText {
                                         Id = _textId,
                                         KeyId = _keyId,
                                         LanguageId = _languageId
                                     };
        }

    }
}