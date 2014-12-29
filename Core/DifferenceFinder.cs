using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core {
    public class DifferenceFinder : IDifferenceFinder {
        public string LanguageChanges(ILoadedLocalization previousLocalization,
                                      ILocalization currentLocalization) {
            string difference = string.Empty;
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
                builder.Append(string.Format(@"{0} language(s) have been added.",
                                             added));

            int removed = previousLanguages.Count(c => !currentLanguagesDictionary.ContainsKey(c.Id));
            if (removed != 0)
                builder.AppendLine(string.Format(@"{0} language(s) have been removed.",
                                                 removed));
            int changed = currentLanguages.Where(c => previousLanguagesDictionary.ContainsKey(c.Id))
                                          .Select(c => c.Id)
                                          .Count(c => !AreEqual(previousLanguagesDictionary[c],
                                                                currentLanguagesDictionary[c]));
            if (changed != 0)
                builder.AppendLine(string.Format(@"{0} language(s) have been changed.",
                                                 changed));
            return builder.ToString();
        }

        private bool AreEqual(Language x,
                              Language y) {
            if (x == y)
                throw new NotSupportedException("This method compares objects by value");
            if (x.Id == y.Id && x.IsoName == y.IsoName && x.DisplayName == y.DisplayName)
                return true;

            return false;
        }
    }

    public interface IDifferenceFinder {
        string LanguageChanges(ILoadedLocalization previousLocalization,
                               ILocalization currentLocalization);
    }
}