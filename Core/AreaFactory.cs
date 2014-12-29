using System;

namespace Core {
    public class AreaFactory : IAreaFactory {
        private readonly IGuidGenerator _guidGenerator;

        public AreaFactory(IGuidGenerator guidGenerator) {
            _guidGenerator = guidGenerator;
        }

        public Area Create(string name) {
            if (name == null)
                name = string.Empty;

            name = name.Trim();

            return new Area {
                                Id = _guidGenerator.Next(),
                                Name = name
                            };
        }
    }

    public interface IAreaFactory {
        Area Create(string name);
    }
}