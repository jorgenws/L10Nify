using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Core {
    public class BuildJsonResourceFileVisitor : ILocalizationVisitor {
        private readonly string _filePath;

        public BuildJsonResourceFileVisitor(string filePath) {
            _filePath = filePath;
        }

        public void Visit(ILocalization localization) {
            var languageResources = new List<LanguageResource>();
            foreach (Language language in localization.RetriveLanguages()) {
                var localizationKeyResources = new List<LocalizationKeyResource>();
                foreach (Area area in localization.RetriveAreas()) {
                    foreach (LocalizationKey key in localization.RetriveKeys()
                                                                .Where(c => c.AreaId == area.Id)
                                                                .ToList()) {
                        var text = localization.RetriveText(key.Id,
                                                            language.Id);
                        localizationKeyResources.Add(new LocalizationKeyResource {
                                                                                     Key = string.Format("{0}.{1}",
                                                                                                         area.Name,
                                                                                                         key.Key),
                                                                                     Text = text == null ? "no text added yet" : text.Text
                                                                                 });
                    }
                }
                languageResources.Add(new LanguageResource {
                                                               LanguageRegion = language.LanguageRegion,
                                                               Keys = localizationKeyResources.ToArray()
                                                           });
            }

            using (var fs = File.Open(_filePath,
                                      FileMode.Create))
            using (var sw = new StreamWriter(fs))
            using (var jw = new JsonTextWriter(sw)) {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(jw,
                                     languageResources);
            }
        }
    }

    public class LanguageResource {
        public string LanguageRegion { get; set; }
        public LocalizationKeyResource[] Keys { get; set; }
        
    }

    public class LocalizationKeyResource {
        public string Key { get; set; }
        public string Text { get; set; }
    }

    public interface ILocalizationVisitor {
        void Visit(ILocalization localization);
    }
}
