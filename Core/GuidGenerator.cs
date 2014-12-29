using System;

namespace Core {
    public class GuidGenerator : IGuidGenerator {
        public Guid Next() {
            return Guid.NewGuid();
        }
    }

    public interface IGuidGenerator {
        Guid Next();
    }
}