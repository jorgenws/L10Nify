﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using L10Nify;
using L10Nify.ViewModel;
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
        private readonly Guid _textId = Guid.Parse("{9C34D957-7A20-410B-A301-8ED406C4E6DC}");

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
        public void Build_HasOneAreaOneKeyAndOneLanguage_ReturnsTreeWithOneAreaAndOneKeyAndOneLanguage() {
            _queryModel.Setup(c => c.RetriveAreas())
                       .Returns(new[] {
                                          CreateDefaultArea()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizationKeys())
                       .Returns(new[] {
                                          CreateDefaultLocalizationKey()
                                      });
            _queryModel.Setup(c => c.RetriveLanguages())
                       .Returns(new[] {
                                          CreateDefaultLanguage()
                                      });

            var builder = CreateDefaultTreeViewModelBuilder();

            var result = builder.Build(_queryModel.Object);

            var areaVM = result.First() as AreaTreeNodeViewModel;
            var keyVM = areaVM.Children.First() as LocalizationKeyTreeNodeViewModel;
            Assert.IsInstanceOf<LanguageTreeNodeViewModel>(keyVM.Children.First());
            var languageVM = keyVM.Children.First() as LanguageTreeNodeViewModel;
            Assert.AreEqual(_languageId,
                            languageVM.Id);
        }

        [Test]
        public void Build_HasOneAreaOneKeyOneLanguageAndOneText_ReturnsTreeWithOneAreaAndOneKeyOneLanguageOneText() {
            _queryModel.Setup(c => c.RetriveAreas())
           .Returns(new[] {
                                          CreateDefaultArea()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizationKeys())
                       .Returns(new[] {
                                          CreateDefaultLocalizationKey()
                                      });
            _queryModel.Setup(c => c.RetriveLanguages())
                       .Returns(new[] {
                                          CreateDefaultLanguage()
                                      });
            _queryModel.Setup(c => c.RetriveLocalizedTexts())
                       .Returns(new[] {
                                          CreateDefaultLocalizedText()
                                      });

            var builder = CreateDefaultTreeViewModelBuilder();

            var result = builder.Build(_queryModel.Object);

            var areaVM = result.First() as AreaTreeNodeViewModel;
            var keyVM = areaVM.Children.First() as LocalizationKeyTreeNodeViewModel;
            var languageVM = keyVM.Children.First() as LanguageTreeNodeViewModel;
            Assert.IsInstanceOf<LocalizedTextTreeNodeViewModel>(languageVM.Children.First());
            var textVM = languageVM.Children.First() as LocalizedTextTreeNodeViewModel;
            Assert.AreEqual(_textId,
                            textVM.Id);
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