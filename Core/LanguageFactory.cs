using System;

namespace Core {
    public class LanguageFactory : ILanguageFactory {
        public Language Create(Guid languageId,
                               string isoName,
                               string displayName) {
            if (languageId == Guid.Empty)
                throw new NotSupportedException("Language id  cannot be empty");

            return new Language {
                                    Id = languageId,
                                    IsoName = isoName,
                                    DisplayName = displayName
                                };
        }
    }

    public interface ILanguageFactory {
        Language Create(Guid languageId,
                        string isoName,
                        string displayName);
    }
}