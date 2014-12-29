using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core {
    public class DifferenceFinder : IDifferenceFinder {
        public string Changes(ILoadedLocalization previousLocalization,
                              ILocalization currentLocalization) {
            var builder = new StringBuilder();

            builder.AppendLine(Difference(previousLocalization.Areas,
                                          currentLocalization.RetriveAreas()
                                                             .ToList(),
                                          a => a.Id,
                                          AreAreasEqual,
                                          "area"));

            builder.AppendLine(Difference(previousLocalization.Keys,
                                          currentLocalization.RetriveKeys()
                                                             .ToList(),
                                          k => k.Id,
                                          AreKeysEqual,
                                          "key"));

            builder.AppendLine(Difference(previousLocalization.Texts,
                                          currentLocalization.RetriveTexts()
                                                             .ToList(),
                                          t => t.Id,
                                          AreTextsEqual,
                                          "text"));

            builder.AppendLine(Difference(previousLocalization.Languages,
                                          currentLocalization.RetriveLanguages()
                                                             .ToList(),
                                          l => l.Id,
                                          AreLanguagesEqual,
                                          "language"));
            return builder.ToString();
        }

        private bool AreLanguagesEqual(Language x,
                                       Language y) {
            if (x == y)
                throw new NotSupportedException("This method compares objects by value");
            return x.Id == y.Id && x.IsoName == y.IsoName && x.DisplayName == y.DisplayName;
        }

        private bool AreAreasEqual(Area x,
                                   Area y) {
            if (x == y)
                throw new NotSupportedException("This method compares objects by value");
            return x.Id == y.Id && x.Name == y.Name;
        }

        private bool AreKeysEqual(LocalizationKey x,
                                  LocalizationKey y) {
            if (x == y)
                throw new NotSupportedException("This metod compares by value");
            return x.Id == y.Id && x.AreaId == y.AreaId && x.Key == y.Key;
        }

        private bool AreTextsEqual(LocalizedText x,
                                   LocalizedText y) {
            if (x == y)
                throw new NotImplementedException("This method compares by value");

            return x.Id == y.Id && x.Text == y.Text && x.KeyId == y.KeyId && x.LanguageId == y.LanguageId;
        }

        private string Difference<T>(List<T> previous,
                                     List<T> current,
            Func<T,Guid> toId,
            Func<T,T,bool> areEqual,
                                     string type) {
            var previousDictionary = previous.ToDictionary(toId);
            var currentDictionary = current.ToDictionary(toId);

            var builder = new StringBuilder();

            int added = current.Count(c => !previousDictionary.ContainsKey(toId(c)));
            if (added != 0)
                builder.AppendLine(string.Format(@"{0} {1}(s) have been added.",
                                                 added,
                                                 type));

            int removed = previous.Count(c => !currentDictionary.ContainsKey(toId(c)));
            if (removed != 0)
                builder.AppendLine(string.Format(@"{0} {1}(s) have been removed.",
                                                 removed,
                                                 type));

            int changes = current.Where(c => previousDictionary.ContainsKey(toId(c)))
                                     .Select(toId)
                                     .Count(c => !areEqual(previousDictionary[c],
                                                                currentDictionary[c]));
            if (changes != 0) {
                builder.AppendLine(string.Format(@"{0} {1}(s) have been changed.",
                                                 changes,
                                                 type));
            }

            return builder.ToString();
        }
    }

    public interface IDifferenceFinder {
        string Changes(ILoadedLocalization previousLocalization,
                       ILocalization currentLocalization);
    }
}