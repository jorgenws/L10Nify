using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Core {
    public class JsonLocalizationLoader : ILocalizationLoader {
        public LoadedLocalization Load(string filePath) {
            LocalizationPersistance persistance = null;
            using (var fs = File.Open(filePath,
                                      FileMode.Open))
            using (var sr = new StreamReader(fs))
            using (var jr = new JsonTextReader(sr)) {
                var serializer = new JsonSerializer();
                persistance = serializer.Deserialize<LocalizationPersistance>(jr);
            }

            return new LoadedLocalization(filePath,
                                          persistance.Languages.ToList(),
                                          persistance.HistoryEntries.ToList());
        }
    }

    public interface ILocalizationLoader {
        LoadedLocalization Load(string filePath);
    }
}