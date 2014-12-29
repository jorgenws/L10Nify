using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Core {
    public class JsonLocalizationPersister : ILocalizationPersister {
        public void Write(string filePath,
                          ILocalization localization) {
            var persitance = new LocalizationPersistance {
                                                             Languages = localization.Languages()
                                                                                     .ToArray(),
                                                             HistoryEntries = localization.History()
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
    }

    public interface ILocalizationPersister {
        void Write(string fileName,
                   ILocalization localization);
    }
}