using System;
using Core;

namespace L10Nify {
    public class MissingLocalizedTextViewModel {
        public string AreaKeyLanguage {
            get {
                return string.Format("{0} {1} {2}",
                                     AreaName,
                                     KeyName,
                                     LanguageName);
            }
        }

        public Guid AreaId { get { return _missingLocalizedText.AreaId; } }
        public string AreaName {
            get {
                return _queryModel.RetriveArea(_missingLocalizedText.AreaId)
                                  .Name;
            }
        }

        public Guid KeyId { get { return _missingLocalizedText.KeyId; } }
        public string KeyName {
            get {
                return _queryModel.RetriveLocalizationKey(_missingLocalizedText.KeyId)
                                  .Key;
            }
        }

        public Guid LanguageId { get { return _missingLocalizedText.LanguageId; } }
        public string LanguageName {
            get {
                return _queryModel.RetriveLanguage(_missingLocalizedText.LanguageId)
                                  .DisplayName;
            }
        }

        private readonly MissingLocalizedText _missingLocalizedText;
        private readonly IQueryModel _queryModel;

        public MissingLocalizedTextViewModel(MissingLocalizedText missingLocalizedText,
                                             IQueryModel queryModel) {
            _missingLocalizedText = missingLocalizedText;
            _queryModel = queryModel;
        }
    }
}