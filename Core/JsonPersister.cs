using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Core {
    public class JsonLocalizationPersister : ILocalizationPersister {
        public void Write(string filePath,
                          ILocalization localization) {
            var persitance = new LocalizationPersistance {
                                                             Areas = localization.RetriveAreas()
                                                                                 .Select(CreatePersitance)
                                                                                 .ToArray(),
                                                             Keys = localization.RetriveKeys()
                                                                                .ToArray(),
                                                             Texts = localization.RetriveTexts()
                                                                                 .ToArray(),

                                                             Languages = localization.RetriveLanguages()
                                                                                     .ToArray(),
                                                             HistoryEntries = localization.RetriveHistory()
                                                                                          .ToArray()
                                                         };

            using (var fs = File.Open(filePath, FileMode.CreateNew)) {
                using (var sw = new StreamWriter(fs)) {
                    using (var jw = new JsonTextWriter(sw)) {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jw,
                                             persitance);
                    }
                }
            }       
        }

        private AreaPersitance CreatePersitance(Area area) {
            return new AreaPersitance {
                                          Id = area.Id,
                                          Name = area.Name,
                                          Comment = area.Comment,
                                          ImageAsBase64String = area.Image != null ? Convert.ToBase64String(area.Image) : null
                                      };
        }
    }

    public interface ILocalizationPersister {
        void Write(string fileName,
                   ILocalization localization);
    }
}