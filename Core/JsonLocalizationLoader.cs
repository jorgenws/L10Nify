using System;

namespace Core {
    public class JsonLocalizationLoader : ILocalizationLoader {
        public LoadedLocalization Load(string filePath) {
            throw new NotImplementedException();
        }
    }

    public interface ILocalizationLoader {
        LoadedLocalization Load(string filePath);
    }
}