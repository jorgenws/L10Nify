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

        public string LanguageRegion { get; set; }
        public int LCID { get; set; }

        private CultureInfo _selectedLocale;
        private string _languageDisplayName;

        public CultureInfo SelectedLocale {
            get { return _selectedLocale; }
            set {
                _selectedLocale = value;
                LanguageDisplayName = _selectedLocale.DisplayName;
                var region = new RegionInfo(_selectedLocale.LCID);
                LanguageRegion = string.Format("{0}-{1}",
                                               _selectedLocale.TwoLetterISOLanguageName,
                                               region.TwoLetterISORegionName);
                LCID = _selectedLocale.LCID;
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
            if (string.IsNullOrWhiteSpace(LanguageRegion)) return;

            TryClose(true);
        }

        public void Cancel() {
            TryClose(false);
        }

        public void SetCultureInfo(int lcid) {
            SelectedLocale = Locales.Single(c => c.LCID == lcid);
        }
    }
}