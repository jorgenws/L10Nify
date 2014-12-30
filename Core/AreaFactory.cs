using System;

namespace Core {
    public class AreaFactory : IAreaFactory {
        public Area Create(Guid areaId,
                           string name) {
            if (areaId == Guid.Empty)
                throw new NotSupportedException("Area id  cannot be empty");

            if (name == null)
                name = string.Empty;

            name = name.Trim();

            return new Area {
                                Id = areaId,
                                Name = name
                            };
        }
    }

    public interface IAreaFactory {
        Area Create(Guid areaId,
                    string name);
    }
}