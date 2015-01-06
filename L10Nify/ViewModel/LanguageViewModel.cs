using System;
using Core;

namespace L10Nify {
    public class LanguageViewModel {
        private readonly Language _language;

        public LanguageViewModel(Language language) {
            _language = language;            
        }

        public Guid Id {
            get { return _language.Id; }
        }

        public string DisplayName { 
            get { return _language.DisplayName; } 
            set { _language.DisplayName = value; } 
        }

        public string ListDisplayName {
            get {
                string listDisplayName;
                if (_language.IsDefault)
                    listDisplayName = string.Format("{0} (is default)",
                                                    _language.DisplayName);
                else
                    listDisplayName = _language.DisplayName;

                return listDisplayName;
            }
        }

        public int LCID {
            get { return _language.LCID; }
            set { _language.LCID = value; }
        }

        public string LanguageRegion {
            get { return _language.LanguageRegion; }
            set { _language.LanguageRegion = value; }
        }
    }
}