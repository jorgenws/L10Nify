using System;
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
                                          persistance.Areas.Select(CreateArea)
                                                     .ToList(),
                                          persistance.Keys.ToList(),
                                          persistance.Texts.ToList(),
                                          persistance.Languages.ToList(),
                                          persistance.HistoryEntries.ToList());
        }

        private Area CreateArea(AreaPersitance areaPersitance) {
            return new Area {
                                Id = areaPersitance.Id,
                                Name = areaPersitance.Name,
                                Comment = areaPersitance.Comment,
                                Image = Convert.FromBase64String(areaPersitance.ImageAsBase64String)
                            };
        }
    }

    public interface ILocalizationLoader {
        LoadedLocalization Load(string filePath);
    }
}