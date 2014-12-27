using System;

namespace Core {
    public class LanguageFactory : ILanguageFactory {
        public Language Create(Guid id,
                               string isoName,
                               string displayName) {
            if (id == Guid.Empty)
                throw new NotSupportedException("Does not support using empty id");

            return new Language {
                                    Id = id,
                                    IsoName = isoName,
                                    DisplayName = displayName
                                };
        }
    }

    public interface ILanguageFactory {
        Language Create(Guid id,
                        string isoName,
                        string displayName);
    }
}