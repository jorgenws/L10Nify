using System;

namespace Core {
    public class LocalizationKeyFactory : ILocalizationKeyFactory {
        private readonly IGuidGenerator _guidGenerator;

        public LocalizationKeyFactory(IGuidGenerator guidGenerator) {
            _guidGenerator = guidGenerator;
        }

        public LocalizationKey Create(Guid areaId,
                                      string key) {
            if (areaId == Guid.Empty)
                throw new NotSupportedException("There must be a area id set");

            if (key == null)
                key = string.Empty;

            key = key.Trim();

            return new LocalizationKey {
                                           Id = _guidGenerator.Next(),
                                           Key = key,
                                           AreaId = areaId
                                       };
        }
    }

    public interface ILocalizationKeyFactory {
        LocalizationKey Create(Guid areaId,
                               string key);
    }
}