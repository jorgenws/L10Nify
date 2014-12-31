using System.Collections.Generic;
using System.Globalization;
using Caliburn.Micro;

namespace L10Nify {
    public class AddLanguageViewModel : Screen {
        public string LanguageDisplayName {
            get { return _languageDisplayName; }
            set {
                if (value == _languageDisplayName) return;
                _languageDisplayName = value;
                NotifyOfPropertyChange(() => LanguageDisplayName);
            }
        }

        public string IsoName {get { return SelectedLocale.TwoLetterISOLanguageName; }}

        private CultureInfo _selectedLocale;
        private string _languageDisplayName;

        public CultureInfo SelectedLocale {
            get { return _selectedLocale; }
            set {
                _selectedLocale = value;
                LanguageDisplayName = _selectedLocale.DisplayName;
            }
        }

        public IEnumerable<CultureInfo> Locales {
            get { return CultureInfo.GetCultures(CultureTypes.NeutralCultures); }
        }

        public void Ok() {
            if (SelectedLocale == null) return;

            TryClose();
        }

        public void Cancel() {
            TryClose();
        }
    }
}