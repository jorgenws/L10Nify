using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core {
    public class DifferenceFinder : IDifferenceFinder {
        public string Changes(ILoadedLocalization previousLocalization,
                              ILocalization currentLocalization) {
            string difference = string.Empty;
            
            difference += AreaDifference(previousLocalization.Areas,
                             currentLocalization.RetriveAreas()
                                                .ToList());

            difference += LanguageDifference(previousLocalization.Languages,
                                             currentLocalization.RetriveLanguages()
                                                                .ToList());
            return difference;
        }

        private string LanguageDifference(List<Language> previousLanguages,
                                          List<Language> currentLanguages) {
            var previousLanguagesDictionary = previousLanguages.ToDictionary(c => c.Id);
            var currentLanguagesDictionary = currentLanguages.ToDictionary(c => c.Id);

            var builder = new StringBuilder();

            int added = currentLanguages.Count(c => !previousLanguagesDictionary.ContainsKey(c.Id));
            if (added != 0)
                builder.AppendLine(string.Format(@"{0} language(s) have been added.",
                                             added));

            int removed = previousLanguages.Count(c => !currentLanguagesDictionary.ContainsKey(c.Id));
            if (removed != 0)
                builder.AppendLine(string.Format(@"{0} language(s) have been removed.",
                                                 removed));
            int changed = currentLanguages.Where(c => previousLanguagesDictionary.ContainsKey(c.Id))
                                          .Select(c => c.Id)
                                          .Count(c => !AreLanguagesEqual(previousLanguagesDictionary[c],
                                                                         currentLanguagesDictionary[c]));
            if (changed != 0)
                builder.AppendLine(string.Format(@"{0} language(s) have been changed.",
                                                 changed));
            return builder.ToString();
        }

        private bool AreLanguagesEqual(Language x,
                                       Language y) {
            if (x == y)
                throw new NotSupportedException("This method compares objects by value");
            if (x.Id == y.Id && x.IsoName == y.IsoName && x.DisplayName == y.DisplayName)
                return true;

            return false;
        }

        private string AreaDifference(List<Area> previousArea,
                                      List<Area> currentArea) {
            var previousAreaDictionary = previousArea.ToDictionary(c => c.Id);
            var currentAreaDictionary = currentArea.ToDictionary(c => c.Id);

            var builder = new StringBuilder();

            int added = currentArea.Count(c => !previousAreaDictionary.ContainsKey(c.Id));
            if (added != 0)
                builder.AppendLine(string.Format(@"{0} area(s) have been added.",
                                                 added));

            int removed = previousArea.Count(c => !currentAreaDictionary.ContainsKey(c.Id));
            if (removed != 0)
                builder.AppendLine(string.Format(@"{0} area(s) have been removed.",
                                                 removed));

            int changes = currentArea.Where(c => previousAreaDictionary.ContainsKey(c.Id))
                                     .Select(c => c.Id)
                                     .Count(c => !AreAreasEqual(previousAreaDictionary[c],
                                                                currentAreaDictionary[c]));
            if (changes!=0) {
                builder.AppendLine(string.Format(@"{0} area(s) have been changed.",
                                                 changes));
            }

            return builder.ToString();
        }

        private bool AreAreasEqual(Area x,
                                   Area y) {
            if (x == y)
                throw new NotSupportedException("This method compares objects by value");
            if (x.Id == y.Id && x.Name == y.Name)
                return true;

            return false;
        }
    }

    public interface IDifferenceFinder {
        string Changes(ILoadedLocalization previousLocalization,
                       ILocalization currentLocalization);
    }
}