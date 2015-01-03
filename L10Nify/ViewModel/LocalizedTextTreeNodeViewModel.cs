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

        public Guid LanguageId {get { return _localizedText.LanguageId; }}

        public string LocalizedText {
            get { return _localizedText.Text; }
        }

        public string DisplayText { get {
            return string.Format("{0} ({1})",
                                 _localizedText.Text,
                                 _language.DisplayName);
        } }

        public string LanguageName { get { return _language.DisplayName; } }

        private readonly LocalizedText _localizedText;
        private readonly Language _language;

        public LocalizedTextTreeNodeViewModel(LocalizedText localizedText, Language language) {
            _localizedText = localizedText;
            _language = language;
        }
    }
}