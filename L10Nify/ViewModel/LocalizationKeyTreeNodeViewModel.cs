using System;
using System.Collections.Generic;
using Core;

namespace L10Nify {
    public class LocalizationKeyTreeNodeViewModel : ITreeNodeViewModel {
        public IEnumerable<ITreeNodeViewModel> Children {
            get { return _localizedTextTreeNodeViewModels; }
        }

        public Guid Id {
            get { return _localizationKey.Id; }
        }

        public Guid AreaId {
            get { return _localizationKey.AreaId; }
        }

        public string Key {
            get { return _localizationKey.Key; }
        }

        private readonly LocalizationKey _localizationKey;
        private readonly IEnumerable<LocalizedTextTreeNodeViewModel> _localizedTextTreeNodeViewModels;

        public LocalizationKeyTreeNodeViewModel(LocalizationKey localizationKey,
                                                IEnumerable<LocalizedTextTreeNodeViewModel> localizedTextTreeNodeViewModels) {
            _localizationKey = localizationKey;
            _localizedTextTreeNodeViewModels = localizedTextTreeNodeViewModels;
        }
    }
}