using System.Collections.Generic;
using System.Linq;
using Core;

namespace L10Nify {
    public class TreeViewModelBuilder : ITreeViewModelBuilder {
        private readonly IAreaViewModelFactory _areaViewModelFactory;

        public TreeViewModelBuilder(IAreaViewModelFactory areaViewModelFactory) {
            _areaViewModelFactory = areaViewModelFactory;
        }

        //public IEnumerable<ITreeNodeViewModel> Build(IQueryModel queryModel) {
        //    var treeNodes = new List<ITreeNodeViewModel>();

        //    foreach (Area area in queryModel.RetriveAreas()) {
        //        var localizationKeyNodes = new List<LocalizationKeyTreeNodeViewModel>();
        //        foreach (LocalizationKey key in queryModel.RetriveLocalizationKeys()
        //                                                  .Where(c => c.AreaId == area.Id)
        //                                                  .ToList()) {
        //            var languageTreeNodes = new List<LanguageTreeNodeViewModel>();
        //            foreach (Language language in queryModel.RetriveLanguages()) {
        //                var textTreeNodes = new List<LocalizedTextTreeNodeViewModel>();
        //                foreach (LocalizedText text in queryModel.RetriveLocalizedTexts()
        //                                                         .Where(c => c.KeyId == key.Id && c.LanguageId == language.Id))
        //                    textTreeNodes.Add(new LocalizedTextTreeNodeViewModel(text));

        //                languageTreeNodes.Add(new LanguageTreeNodeViewModel(language,
        //                                                                    textTreeNodes));
        //            }

        //            localizationKeyNodes.Add(new LocalizationKeyTreeNodeViewModel(key,
        //                                                                          languageTreeNodes));
        //        }

        //        treeNodes.Add(new AreaTreeNodeViewModel(_areaViewModelFactory.Create(area),
        //                                                localizationKeyNodes));
        //    }

        //    return treeNodes;
        //}

        public IEnumerable<ITreeNodeViewModel> Build(IQueryModel queryModel) {
            var areaNodes = new List<ITreeNodeViewModel>();

            foreach (Area area in queryModel.RetriveAreas()) {
                var localizationKeyNodes = new List<LocalizationKeyTreeNodeViewModel>();
                foreach (LocalizationKey key in queryModel.RetriveLocalizationKeys()
                                                          .Where(c => c.AreaId == area.Id)
                                                          .ToList()) {
                    var localizedTextNodes = new List<LocalizedTextTreeNodeViewModel>();
                    foreach (LocalizedText localizedText in queryModel.RetriveLocalizedTexts()
                                                                      .Where(c => c.KeyId == key.Id)
                                                                      .ToList()) {
                        localizedTextNodes.Add(new LocalizedTextTreeNodeViewModel(localizedText,
                                                                                  queryModel.RetriveLanguage(localizedText.LanguageId)));
                    }
                    localizationKeyNodes.Add(new LocalizationKeyTreeNodeViewModel(key,
                                                                                  localizedTextNodes));
                }
                areaNodes.Add(new AreaTreeNodeViewModel(_areaViewModelFactory.Create(area),
                                                        localizationKeyNodes));
            }
            return areaNodes;
        }
    }

    public interface ITreeViewModelBuilder {
        IEnumerable<ITreeNodeViewModel> Build(IQueryModel queryModel);
    }
}