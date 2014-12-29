using System;

namespace Core {
    public class LocalizedTextFactory : ILocalizedTextFactory {
        private readonly IGuidGenerator _guidGenerator;
        
        public LocalizedTextFactory(IGuidGenerator guidGenerator) {
            _guidGenerator = guidGenerator;
        }

        public LocalizedText Create(Guid keyId,
                                    Guid languageid,
                                    string text) {
            if(keyId == Guid.Empty)
                throw new NotSupportedException("Must have a key id");

            if (languageid == Guid.Empty)
                throw new NotSupportedException("Must have a language id");

            if (text == null)
                text = string.Empty;

            return new LocalizedText {
                                         Id = _guidGenerator.Next(),
                                         KeyId = keyId,
                                         LanguageId = languageid,
                                         Text = text
                                     };
        }
    }

    public interface ILocalizedTextFactory {
        LocalizedText Create(Guid keyId,
                             Guid languageid,
                             string text);
    }
}