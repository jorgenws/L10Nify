using System;

namespace Core {
    public class JsonLocalizationPersister : ILocalizationPersister {
        public void Write(string fileName,
                          ILocalization localization) {
            //ToDo: Implement persisting to disk in JSON format.
            throw new NotImplementedException();
        }
    }

    public interface ILocalizationPersister {
        void Write(string fileName,
                   ILocalization localization);
    }
}