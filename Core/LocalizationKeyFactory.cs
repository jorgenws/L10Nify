using System;

namespace Core {
    public class LocalizationKeyFactory : ILocalizationKeyFactory {
        public LocalizationKey Create(Guid areaId,
                                      Guid keyId,
                                      string key) {
            if (areaId == Guid.Empty)
                throw new NotSupportedException("There must be a area id set");
            if (keyId == Guid.Empty)
                throw new NotSupportedException("There must be a localization key id set");

            if (key == null)
                key = string.Empty;

            key = key.Trim();

            return new LocalizationKey {
                                           Id = keyId,
                                           Key = key,
                                           AreaId = areaId
                                       };
        }
    }

    public interface ILocalizationKeyFactory {
        LocalizationKey Create(Guid areaId,
                               Guid keyId,
                               string key);
    }
}