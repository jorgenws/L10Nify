using System;
using System.Collections.Generic;
using Core;

namespace L10Nify {
    public class LocalizedTextTreeNodeViewModel : ITreeNodeViewModel {
        public IEnumerable<ITreeNodeViewModel> Children {
            get { return new ITreeNodeViewModel[0]; }
        }

        public Guid Id {
            get { return _localizedText.Id; }
        }

        public Guid KeyId {
            get { return _localizedText.KeyId; }
        }

        public string LocalizedText {
            get { return _localizedText.Text; }
        }

        private readonly LocalizedText _localizedText;

        public LocalizedTextTreeNodeViewModel(LocalizedText localizedText) {
            _localizedText = localizedText;
        }
    }
}