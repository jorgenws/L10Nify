using System;

namespace Core {
    public class LocalizedTextFactory : ILocalizedTextFactory {
        public LocalizedText Create(Guid textId,
                                    Guid keyId,
                                    Guid languageid,
                                    string text) {
            if (textId == Guid.Empty)
                throw new NotSupportedException("Must have a text id");
            if (keyId == Guid.Empty)
                throw new NotSupportedException("Must have a key id");
            if (languageid == Guid.Empty)
                throw new NotSupportedException("Must have a language id");

            if (text == null)
                text = string.Empty;

            return new LocalizedText {
                                         Id = textId,
                                         KeyId = keyId,
                                         LanguageId = languageid,
                                         Text = text
                                     };
        }
    }

    public interface ILocalizedTextFactory {
        LocalizedText Create(Guid textId,
                             Guid keyId,
                             Guid languageid,
                             string text);
    }
}