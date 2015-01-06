using System;

namespace Core {
    public class LanguageFactory : ILanguageFactory {
        public Language Create(Guid languageId,
                               string languageRegion,
                               int lcid,
                               string displayName) {
            if (languageId == Guid.Empty)
                throw new NotSupportedException("Language id  cannot be empty");

            return new Language {
                                    Id = languageId,
                                    LCID = lcid,
                                    LanguageRegion = languageRegion,
                                    DisplayName = displayName,
                                    IsDefault = false
                                };
        }
    }

    public interface ILanguageFactory {
        Language Create(Guid languageId,
                        string languageRegion,
                        int lcid,
                        string displayName);
    }
}