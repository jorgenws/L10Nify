using System;

namespace Core {
    public class LocalizationVisitorFactory : ILocalizationVisitorFactory {
        public ILocalizationVisitor Create(ResourceType resourceType,
                                           string filePath) {
            if (resourceType == ResourceType.Json)
                return new BuildJsonResourceFileVisitor(filePath);
            if (resourceType == ResourceType.DotNet)
                return new BuildDotNetResourceFilesVisitor(filePath);

            throw new NotImplementedException(string.Format("There is no implementation for {0}",
                                                            resourceType));
        }
    }

    public interface ILocalizationVisitorFactory {
        ILocalizationVisitor Create(ResourceType resourceType,
                                    string filePath);
    }

    public enum ResourceType {
        Json,
        DotNet
    }
}