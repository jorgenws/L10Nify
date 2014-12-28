using System;

namespace Core {
    public class JsonPersister : IPersister {
        public void Write(string fileName,
                         Localization localization) {
            //ToDo: Implement persisting to disk in JSON format.
            throw new NotImplementedException();
        }
    }

    public interface IPersister {
        void Write(string fileName,
                  Localization localization);
    }
}