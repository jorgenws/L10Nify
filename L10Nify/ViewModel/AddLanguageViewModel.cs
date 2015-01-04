using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public string IsoName {
            get { return SelectedLocale.Name; }
            set { SelectedLocale = Locales.Single(c => c.Name == value); }
        }

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
            get {
                return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                  .OrderBy(c => c.EnglishName)
                                  .Select(c => CultureInfo.CreateSpecificCulture(c.Name));
            }
        }

        public void Ok() {
            if (SelectedLocale == null) return;

            TryClose(true);
        }

        public void Cancel() {
            TryClose(false);
        }
    }
}