using System;
using System.Collections.Generic;
using Core;

namespace L10Nify {
    public class LanguageTreeNodeViewModel : ITreeNodeViewModel {
        public IEnumerable<ITreeNodeViewModel> Children {
            get { return _localizedTextTreeNodeViewModels; }
        }

        public Guid Id {
            get { return _language.Id; }
        }

        public string DisplayName {
            get { return _language.DisplayName; }
        }

        private readonly Language _language;
        private readonly IEnumerable<LocalizedTextTreeNodeViewModel> _localizedTextTreeNodeViewModels;

        public LanguageTreeNodeViewModel(Language language,
                                         IEnumerable<LocalizedTextTreeNodeViewModel> localizedTextTreeNodeViewModels) {
            _language = language;
            _localizedTextTreeNodeViewModels = localizedTextTreeNodeViewModels;
        }
    }
}